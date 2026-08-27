using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipRotationStabilizerActivator : MonoBehaviour
{
    [SerializeField] private Ship ship;
    [SerializeField] private InputActionReference rotationStabilizerInput;
    [SerializeField] private Image rotationStabilizerImage;


    void OnEnable()
    {
        rotationStabilizerInput.action.Enable();
        rotationStabilizerInput.action.performed += OnActionPerformed;
    }
    void OnDisable()
    {
        rotationStabilizerInput.action.performed -= OnActionPerformed;
    }


    void Start()
    {
        UpdateImageVisibility();
    }


    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        ship.SetAutoRotation(!ship.IsAutoRotationStabilizerActive);
        UpdateImageVisibility();
    }


    private void UpdateImageVisibility()
    {
        rotationStabilizerImage.gameObject.SetActive(ship.IsAutoRotationStabilizerActive);
    }
}
