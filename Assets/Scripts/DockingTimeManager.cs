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

    private DockingTime startDockingTime;


    [Serializable]
    public struct DockingTime
    {
        [SerializeField, Range(0, 23)] private int hours;
        [SerializeField, Range(0, 59)] private int minutes;
        [SerializeField, Range(0, 59)] private int seconds;

        // public int Hours => hours;
        // public int Minutes => minutes;
        public int Seconds => seconds;

        private int startSeconds;
        private int startMinutes;
        private int startHours;

        public DockingTime(int hours, int minutes, int seconds)
        {
            this.hours = hours;
            this.minutes = minutes + (hours * 60);
            this.seconds = seconds + (minutes * 60) + (hours * 3600);

            startHours = this.hours;
            startMinutes = this.minutes;
            startSeconds = this.seconds;
        }


        // public void SetTime(int hours, int minutes, int seconds)
        // {
        //     this.hours = hours;
        //     this.minutes = minutes;
        //     this.seconds = seconds;
        // }


        public void Tick(float secondsSinceLevelLoad)
        {
            seconds = startSeconds + Mathf.FloorToInt(secondsSinceLevelLoad);
            minutes = startMinutes + Mathf.FloorToInt(secondsSinceLevelLoad / 60f);
            hours = startHours + Mathf.FloorToInt(secondsSinceLevelLoad / 3600f);

            Debug.Log($"hours: {hours}. minutes: {minutes}. seconds: {seconds}.");
        }


        // public int GetTimeInSeconds()
        // {
        //     return (hours * 3600) + (minutes * 60) + seconds;
        // }


        public override string ToString()
        {
            return $"{hours.ToString("00")} : {minutes.ToString("00")} : {seconds.ToString("00")}";
        }
    }


    void Start()
    {
        int startHours = targetTime.Hours - targetDuration.Hours;
        int startMinutes = targetTime.Minutes - targetDuration.Minutes;
        int startSeconds = targetTime.Seconds - targetDuration.Seconds;
        currentDockingTime = new(startHours, startMinutes, startSeconds);
        // startDockingTime = new(startHours, startMinutes, startSeconds);

        targetTimeTmp.SetText("Tt: " + targetTime.ToString());
    }


    void Update()
    {
        // int intSeconds = Mathf.FloorToInt(Mathf.Repeat(Time.timeSinceLevelLoad + startDockingTime.Seconds, 60));
        // int intMinutes = Mathf.FloorToInt(Mathf.Repeat((Time.timeSinceLevelLoad / 60) + startDockingTime.Minutes, 60));
        // int intHours = Mathf.FloorToInt(Mathf.Repeat((Time.timeSinceLevelLoad / 3600) + startDockingTime.Hours, 24));

        currentDockingTime.Tick(Time.timeSinceLevelLoad);

        // currentDockingTime.SetTime(intHours, intMinutes, intSeconds);

        currentTimeTmp.SetText("Ct: " + currentDockingTime.ToString());

    }
}
