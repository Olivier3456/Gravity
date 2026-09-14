using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipStabilizerActivator : MonoBehaviour
{
    public enum ShipStabilizerType { Rotation, Position }

    [SerializeField] private ShipStabilizerType type;
    [SerializeField] private Ship ship;
    [SerializeField] private InputActionReference stabilizerInput;
    [SerializeField] private Image stabilizerImage;

    [SerializeField] private float timeThreshold = 0.5f;


    private float inputTimer;

    private bool IsStabilizerActive
    {
        get
        {
            switch (type)
            {
                case ShipStabilizerType.Rotation:
                    return ship.IsAutoRotationStabilizerActive;
                case ShipStabilizerType.Position:
                    return ship.IsAutoPositionStabilizerActive;
            }
            return false;
        }
    }


    void OnEnable()
    {
        stabilizerInput.action.Enable();
    }
    void OnDisable()
    {
        stabilizerInput.action.Disable();
    }


    void Start()
    {
        UpdateImageVisibility();
    }


    void Update()
    {
        if (stabilizerInput.action.IsPressed())
        {
            if (inputTimer == 0f)
            {
                ToggleStabilizer();
            }

            inputTimer += Time.deltaTime;
        }
        else
        {
            if (inputTimer >= timeThreshold)
            {
                ToggleStabilizer();
            }

            inputTimer = 0f;
        }
    }


    private void ToggleStabilizer()
    {
        switch (type)
        {
            case ShipStabilizerType.Rotation:
                ship.SetAutoRotation(!ship.IsAutoRotationStabilizerActive);
                break;
            case ShipStabilizerType.Position:
                ship.SetAutoPosition(!ship.IsAutoPositionStabilizerActive);
                break;
        }
        UpdateImageVisibility();
    }


    private void UpdateImageVisibility()
    {
        stabilizerImage.gameObject.SetActive(IsStabilizerActive);
    }
}
