using UnityEngine;
using UnityEngine.UI;

public class ShipFuelManager : MonoBehaviour
{
    [SerializeField] private bool hasUnlimitedFuel;
    [SerializeField, Range(0f, 1000f)] private float fuelMaxAmount = 1000f;
    [SerializeField, Range(0f, 1000f)] private float fuelStartAmount = 1000f;

    [SerializeField, Tooltip("Unit per second at full power for each thruster.")] private float positionThrustersConsumption = 1f;
    [SerializeField, Tooltip("Unit per second at full power for each thruster.")] private float rotationThrustersConsumption = 1f;

    [SerializeField] private ShipTranslation shipTranslation;
    [SerializeField] private ShipRotation shipRotation;


    [Space(10)]
    [SerializeField] private Image fuelGaugeImage;

    public float FuelCurrentAmount { get; private set; }

    public bool HasFuel
    {
        get
        {
            return FuelCurrentAmount > 0f || hasUnlimitedFuel;
        }
    }

    void Awake()
    {
        fuelMaxAmount = Mathf.Max(0f, fuelMaxAmount);
        fuelStartAmount = Mathf.Min(fuelMaxAmount, fuelStartAmount);
        FuelCurrentAmount = fuelStartAmount;
    }

    void Start()
    {
        fuelGaugeImage.fillAmount = FuelCurrentAmount / fuelMaxAmount;
    }


    void Update()
    {
        if (hasUnlimitedFuel)
        {
            return;
        }

        if (FuelCurrentAmount == 0f)
        {
            return;
        }

        float currentFuelSpentForPosition = Mathf.Abs(shipTranslation.CurrentForceNormalized.x * positionThrustersConsumption * Time.deltaTime)
                                          + Mathf.Abs(shipTranslation.CurrentForceNormalized.y * positionThrustersConsumption * Time.deltaTime)
                                          + Mathf.Abs(shipTranslation.CurrentForceNormalized.z * positionThrustersConsumption * Time.deltaTime);

        float currentFuelSpentForRotation = Mathf.Abs(shipRotation.CurrentTorqueNormalized.x * rotationThrustersConsumption * Time.deltaTime)
                                          + Mathf.Abs(shipRotation.CurrentTorqueNormalized.y * rotationThrustersConsumption * Time.deltaTime)
                                          + Mathf.Abs(shipRotation.CurrentTorqueNormalized.z * rotationThrustersConsumption * Time.deltaTime);

        FuelCurrentAmount -= currentFuelSpentForPosition + currentFuelSpentForRotation;

        FuelCurrentAmount = Mathf.Max(0f, FuelCurrentAmount);

        fuelGaugeImage.fillAmount = FuelCurrentAmount / fuelMaxAmount;

        if (FuelCurrentAmount == 0f)
        {
            Debug.Log("Out of fuel!");
        }
    }
}
