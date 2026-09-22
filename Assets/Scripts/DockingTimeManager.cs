using System;
using TMPro;
using UnityEngine;

public class DockingTimeManager : MonoBehaviour
{
    [SerializeField] private DockingTime targetTime;
    [SerializeField] private DockingTime targetDuration;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentTimeTmp;
    [SerializeField] private TextMeshProUGUI targetTimeTmp;

    private DockingTime currentDockingTime = new();



    [Serializable]
    public struct DockingTime
    {
        [SerializeField, Range(0, 23)] private int hours;
        [SerializeField, Range(0, 59)] private int minutes;
        [SerializeField, Range(0, 59)] private int seconds;

        public int Hours => hours;
        public int Minutes => minutes;
        public int Seconds => seconds;

        private float lastSecondCountInFloat;

        public DockingTime(int hours, int minutes, int seconds)
        {
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            lastSecondCountInFloat = 0f;
        }


        public void Tick(float deltaTime)
        {
            lastSecondCountInFloat += deltaTime;
            while (lastSecondCountInFloat >= 1f)
            {
                lastSecondCountInFloat--;
                seconds++;
            }

            if (seconds > 59)
            {
                seconds = 0;
                minutes++;
            }
            if (minutes > 59)
            {
                minutes = 0;
                hours++;
            }
            if (hours > 23)
            {
                hours = 0;
            }
        }

        public override string ToString()
        {
            return $"{hours.ToString("00")} : {minutes.ToString("00")} : {seconds.ToString("00")}";
        }
    }


    void Start()
    {
        int startSeconds;
        int startMinutes = 0;
        int startHours = 0;

        startSeconds = targetTime.Seconds - targetDuration.Seconds;
        if (startSeconds < 0)
        {
            startSeconds += 60;
            startMinutes--;
        }

        startMinutes += targetTime.Minutes - targetDuration.Minutes;
        if (startMinutes < 0)
        {
            startMinutes += 60;
            startHours--;
        }

        startHours += targetTime.Hours - targetDuration.Hours;
        if (startHours < 0)
        {
            startHours += 24;
        }

        currentDockingTime = new(startHours, startMinutes, startSeconds);

        currentTimeTmp.SetText("Ct: " + currentDockingTime.ToString());
        targetTimeTmp.SetText("Tt: " + targetTime.ToString());
    }


    void Update()
    {
        currentDockingTime.Tick(Time.deltaTime);
        currentTimeTmp.SetText("Ct: " + currentDockingTime.ToString());
    }
}
