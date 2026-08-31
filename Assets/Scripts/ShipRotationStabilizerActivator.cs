using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipRotationStabilizerActivator : MonoBehaviour
{
    [SerializeField] private Ship ship;
    [SerializeField] private InputActionReference rotationStabilizerInput;
    [SerializeField] private Image rotationStabilizerImage;

    [SerializeField] private float timeThreshold = 0.5f;

    private float inputTimer;

    void OnEnable()
    {
        rotationStabilizerInput.action.Enable();
        // rotationStabilizerInput.action.performed += OnActionPerformed;
    }
    // void OnDisable()
    // {
    //     rotationStabilizerInput.action.performed -= OnActionPerformed;
    // }


    // private void OnActionPerformed(InputAction.CallbackContext context)
    // {
    //     ship.SetAutoRotation(!ship.IsAutoRotationStabilizerActive);
    //     UpdateImageVisibility();
    // }


    void Start()
    {
        UpdateImageVisibility();
    }


    void Update()
    {
        if (rotationStabilizerInput.action.IsPressed())
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
        ship.SetAutoRotation(!ship.IsAutoRotationStabilizerActive);
        UpdateImageVisibility();
    }


    private void UpdateImageVisibility()
    {
        rotationStabilizerImage.gameObject.SetActive(ship.IsAutoRotationStabilizerActive);
    }
}
