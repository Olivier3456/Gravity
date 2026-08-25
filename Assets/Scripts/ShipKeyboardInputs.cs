using UnityEngine;
using UnityEngine.InputSystem;
public class ShipKeyboardInputs : MonoBehaviour, IShipMovementInputs
{
    [SerializeField] private float powerIncrementDuration = 0.5f;
    [SerializeField] private float powerDecrementDuration = 0.1f;


    private float positionAxisXPositive;
    private float positionAxisXNegative;

    private float positionAxisYPositive;
    private float positionAxisYNegative;

    private float positionAxisZPositive;
    private float positionAxisZNegative;


    private float rotationAxisXPositive;
    private float rotationAxisXNegative;

    private float rotationAxisYPositive;
    private float rotationAxisYNegative;

    private float rotationAxisZPositive;
    private float rotationAxisZNegative;


    public float PositionAxisX => positionAxisXPositive - positionAxisXNegative;
    public float PositionAxisY => positionAxisYPositive - positionAxisYNegative;
    public float PositionAxisZ => positionAxisZPositive - positionAxisZNegative;

    public float RotationAxisX => rotationAxisXPositive - rotationAxisXNegative;
    public float RotationAxisY => rotationAxisYPositive - rotationAxisYNegative;
    public float RotationAxisZ => rotationAxisZPositive - rotationAxisZNegative;


    void Update()
    {
        UpdatePositionInputs();
        UpdateRotationInputs();
    }


    private void UpdateInputValue(UnityEngine.InputSystem.Controls.KeyControl keyControlAxis, ref float axisValue)
    {
        if (keyControlAxis.isPressed)
        {
            axisValue = Mathf.Clamp01(axisValue + (Time.deltaTime / powerIncrementDuration));
        }
        else
        {
            axisValue = Mathf.Clamp01(axisValue - (Time.deltaTime / powerDecrementDuration));
        }
    }


    private void UpdatePositionInputs()
    {
        UpdateInputValue(Keyboard.current.dKey, ref positionAxisXPositive);
        UpdateInputValue(Keyboard.current.aKey, ref positionAxisXNegative);

        UpdateInputValue(Keyboard.current.eKey, ref positionAxisYPositive);
        UpdateInputValue(Keyboard.current.xKey, ref positionAxisYNegative);

        UpdateInputValue(Keyboard.current.wKey, ref positionAxisZPositive);
        UpdateInputValue(Keyboard.current.sKey, ref positionAxisZNegative);
    }

    private void UpdateRotationInputs()
    {
        UpdateInputValue(Keyboard.current.upArrowKey, ref rotationAxisXPositive);
        UpdateInputValue(Keyboard.current.downArrowKey, ref rotationAxisXNegative);

        UpdateInputValue(Keyboard.current.numpad0Key, ref rotationAxisYPositive);
        UpdateInputValue(Keyboard.current.rightCtrlKey, ref rotationAxisYNegative);

        UpdateInputValue(Keyboard.current.leftArrowKey, ref rotationAxisZPositive);
        UpdateInputValue(Keyboard.current.rightArrowKey, ref rotationAxisZNegative);
    }
}



// using UnityEngine;

// public class ShipKeyboardInputs : MonoBehaviour, IShipMovementInputs
// {
//     public float positionAxisX { get; private set; }
//     public float positionAxisY { get; private set; }
//     public float positionAxisZ { get; private set; }

//     public float rotationAxisX { get; private set; }
//     public float rotationAxisY { get; private set; }
//     public float rotationAxisZ { get; private set; }


//     void Update()
//     {
//         UpdatePositionInputs();
//         UpdateRotationInputs();
//     }


//     private void UpdatePositionInputs()
//     {
//         positionAxisX = UnityEngine.InputSystem.Keyboard.current.dKey.isPressed ? 1 : 0;                // Right
//         positionAxisX = UnityEngine.InputSystem.Keyboard.current.aKey.isPressed ? -1 : positionAxisX;   // Left

//         positionAxisY = UnityEngine.InputSystem.Keyboard.current.eKey.isPressed ? 1 : 0;                // Up
//         positionAxisY = UnityEngine.InputSystem.Keyboard.current.xKey.isPressed ? -1 : positionAxisY;   // Down

//         positionAxisZ = UnityEngine.InputSystem.Keyboard.current.wKey.isPressed ? 1 : 0;                // Forward
//         positionAxisZ = UnityEngine.InputSystem.Keyboard.current.sKey.isPressed ? -1 : positionAxisZ;   // Backward
//     }

//     private void UpdateRotationInputs()
//     {
//         rotationAxisX = UnityEngine.InputSystem.Keyboard.current.upArrowKey.isPressed ? 1 : 0;
//         rotationAxisX = UnityEngine.InputSystem.Keyboard.current.downArrowKey.isPressed ? -1 : rotationAxisX;

//         rotationAxisY = UnityEngine.InputSystem.Keyboard.current.numpad0Key.isPressed ? 1 : 0;
//         rotationAxisY = UnityEngine.InputSystem.Keyboard.current.ctrlKey.isPressed ? -1 : rotationAxisY;

//         rotationAxisZ = UnityEngine.InputSystem.Keyboard.current.leftArrowKey.isPressed ? 1 : 0;
//         rotationAxisZ = UnityEngine.InputSystem.Keyboard.current.rightArrowKey.isPressed ? -1 : rotationAxisZ;
//     }
// }
