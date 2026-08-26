using UnityEngine;
using UnityEngine.InputSystem;

public class ShipGamepadInputs : MonoBehaviour, IShipMovementInputs
{
    [SerializeField] private InputActionReference leftThumbstickX;
    [SerializeField] private InputActionReference leftThumbstickY;

    [SerializeField] private InputActionReference rightThumbstickX;
    [SerializeField] private InputActionReference rightThumbstickY;

    [SerializeField] private InputActionReference leftTrigger;
    [SerializeField] private InputActionReference rightTrigger;

    [SerializeField] private InputActionReference westButton;
    [SerializeField] private InputActionReference eastButton;


    public float PositionAxisX { get; private set; }
    public float PositionAxisY { get; private set; }
    public float PositionAxisZ { get; private set; }

    public float RotationAxisX { get; private set; }
    public float RotationAxisY { get; private set; }
    public float RotationAxisZ { get; private set; }


    void OnEnable()
    {
        leftThumbstickX.action.Enable();
        leftThumbstickY.action.Enable();

        rightThumbstickX.action.Enable();
        rightThumbstickY.action.Enable();

        westButton.action.Enable();
        eastButton.action.Enable();

        leftTrigger.action.Enable();
        rightTrigger.action.Enable();
    }


    void Update()
    {
        float leftAxisX = leftThumbstickX.action.ReadValue<float>();
        float leftAxisY = leftThumbstickY.action.ReadValue<float>();
        float rightAxisX = rightThumbstickX.action.ReadValue<float>();
        float rightAxisY = rightThumbstickY.action.ReadValue<float>();
        float leftTriggerAxis = leftTrigger.action.ReadValue<float>();
        float rightTriggerAxis = rightTrigger.action.ReadValue<float>();

        PositionAxisX = leftAxisX;
        PositionAxisY = leftAxisY;
        PositionAxisZ = rightTriggerAxis - leftTriggerAxis;

        RotationAxisX = rightAxisY;
        RotationAxisY = rightAxisX;
        RotationAxisZ = westButton.action.ReadValue<float>() - eastButton.action.ReadValue<float>();
    }
}



// public class ShipGamepadInputs : MonoBehaviour, IShipMovementInputs
// {
//     [SerializeField] private InputActionReference leftThumbstickX;
//     [SerializeField] private InputActionReference leftThumbstickY;

//     [SerializeField] private InputActionReference leftTrigger;
//     [SerializeField] private InputActionReference rightTrigger;
//     // [SerializeField] private InputActionReference rightThumbstickX;
//     // [SerializeField] private InputActionReference rightThumbstickY;

//     // [SerializeField] private InputActionReference positionMode;
//     [SerializeField] private InputActionReference rotationMode;


//     public float PositionAxisX { get; private set; }
//     public float PositionAxisY { get; private set; }
//     public float PositionAxisZ { get; private set; }

//     public float RotationAxisX { get; private set; }
//     public float RotationAxisY { get; private set; }
//     public float RotationAxisZ { get; private set; }


//     void OnEnable()
//     {
//         leftThumbstickX.action.Enable();
//         leftThumbstickY.action.Enable();

//         leftTrigger.action.Enable();
//         rightTrigger.action.Enable();
//         // rightThumbstickX.action.Enable();
//         // rightThumbstickY.action.Enable();

//         // positionMode.action.Enable();
//         rotationMode.action.Enable();
//     }


//     void Update()
//     {
//         float leftAxisX = leftThumbstickX.action.ReadValue<float>();
//         float leftAxisY = leftThumbstickY.action.ReadValue<float>();
//         float leftTriggerAxis = leftTrigger.action.ReadValue<float>();
//         float rightTriggerAxis = rightTrigger.action.ReadValue<float>();

//         PositionAxisX = rotationMode.action.ReadValue<float>() < 0.5f ? leftAxisX : 0f;
//         PositionAxisY = rotationMode.action.ReadValue<float>() < 0.5f ? leftAxisY : 0f;
//         PositionAxisZ = rotationMode.action.ReadValue<float>() < 0.5f ? rightTriggerAxis - leftTriggerAxis : 0f;

//         RotationAxisX = rotationMode.action.ReadValue<float>() > 0.5f ? leftAxisY : 0f;
//         RotationAxisY = rotationMode.action.ReadValue<float>() > 0.5f ? leftAxisX : 0f;
//         RotationAxisZ = rotationMode.action.ReadValue<float>() > 0.5f ? leftTriggerAxis - rightTriggerAxis : 0f;
//     }
// }



// using UnityEngine;
// using UnityEngine.InputSystem;

// public class ShipGamepadInputs : MonoBehaviour, IShipMovementInputs
// {
//     [SerializeField] private InputActionReference leftThumbstickX;
//     [SerializeField] private InputActionReference leftThumbstickY;
//     [SerializeField] private InputActionReference rightThumbstickX;
//     [SerializeField] private InputActionReference rightThumbstickY;

//     [SerializeField] private InputActionReference positionMode;
//     [SerializeField] private InputActionReference rotationMode;


//     public float positionAxisX { get; private set; }
//     public float positionAxisY { get; private set; }
//     public float positionAxisZ { get; private set; }

//     public float rotationAxisX { get; private set; }
//     public float rotationAxisY { get; private set; }
//     public float rotationAxisZ { get; private set; }


//     void OnEnable()
//     {
//         leftThumbstickX.action.Enable();
//         leftThumbstickY.action.Enable();
//         rightThumbstickX.action.Enable();
//         rightThumbstickY.action.Enable();
//         positionMode.action.Enable();
//         rotationMode.action.Enable();
//         Debug.Log("Action enabled");
//     }


//     void Update()
//     {
//         float leftAxisX = leftThumbstickX.action.ReadValue<float>();
//         float leftAxisY = leftThumbstickY.action.ReadValue<float>();

//         float rightAxisX = rightThumbstickX.action.ReadValue<float>();
//         float rightAxisY = rightThumbstickY.action.ReadValue<float>();

//         positionAxisX = positionMode.action.ReadValue<float>() > 0.5f ? leftAxisX : 0f;
//         positionAxisY = positionMode.action.ReadValue<float>() > 0.5f ? leftAxisY : 0f;
//         positionAxisZ = positionMode.action.ReadValue<float>() > 0.5f ? rightAxisY : 0f;

//         rotationAxisX = rotationMode.action.ReadValue<float>() > 0.5f ? leftAxisY : 0f;
//         rotationAxisY = rotationMode.action.ReadValue<float>() > 0.5f ? leftAxisX : 0f;
//         rotationAxisZ = rotationMode.action.ReadValue<float>() > 0.5f ? rightAxisX : 0f;

//         // Debug.Log($"leftThumbstickX: {leftThumbstickX.action.ReadValue<float>()}");

//         // Debug.Log($"Position mode: {positionMode.action.ReadValue<float>()}. ---- Rotation mode: {rotationMode.action.ReadValue<float>()}.");

//         Debug.Log($"positionAxisX: {positionAxisX}. positionAxisY: {positionAxisY}. positionAxisZ: ,{positionAxisZ}.");
//     }
// }
