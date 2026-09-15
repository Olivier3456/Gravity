using TMPro;
using UnityEngine;

public class ShipSpeedDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedTMP;
    [SerializeField] private ShipTranslation shipTranslation;
    [SerializeField] private float displayInterval = 0.1f;

    private float displayTimer;

    private const string MS = " m/s";

    void Update()
    {
        displayTimer += Time.deltaTime;

        if (displayTimer < displayInterval)
        {
            return;
        }
        displayTimer = 0f;



        float currentSpeed = shipTranslation.CurrentSpeed;

        if (currentSpeed < 0.01f)
        {
            speedTMP.SetText(currentSpeed.ToString("00.0000") + MS);
        }
        else if (currentSpeed < 0.1)
        {
            speedTMP.SetText(currentSpeed.ToString("00.000") + MS);
        }
        else if (currentSpeed < 1f)
        {
            speedTMP.SetText(currentSpeed.ToString("00.00") + MS);
        }
        else
        {
            speedTMP.SetText(currentSpeed.ToString("00.0") + MS);
        }
    }
}
