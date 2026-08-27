using System;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float mass = 1f;

    [SerializeField] private Vector3 startForceRelative;


    [SerializeField, Space(20)] private ShipKeyboardInputs shipKeyboardInputs;
    [SerializeField] private ShipGamepadInputs shipGamepadInputs;

    [SerializeField, Space(15)] private bool autoRotationStabilizer;


    public enum InputSource { Keyboard, Gamepad };
    [SerializeField, Space(20)] private InputSource inputSource;


    public IShipMovementInputs ShipInputs { get; private set; }
    public Rigidbody Rigidbody => rb;
    public bool AutoRotationStabilizer => autoRotationStabilizer;


    public bool IsCrashed { get; private set; }
    public void Crash()
    {
        IsCrashed = true;
        Debug.Log("Crash!");
    }


    public bool IsDocked { get; private set; }
    public void SetDocked(bool isDocked)
    {
        if (IsCrashed)
        {
            Debug.Log("Can't dock ship if ship is crashed.");
            return;
        }

        IsDocked = isDocked;
        rb.isKinematic = isDocked;
    }

    void Awake()
    {
        if (inputSource == InputSource.Keyboard)
        {
            ShipInputs = shipKeyboardInputs;
            Destroy(shipGamepadInputs);
        }
        else
        {
            ShipInputs = shipGamepadInputs;
            Destroy(shipKeyboardInputs);
        }
    }


    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        rb.centerOfMass = rb.transform.InverseTransformPoint(centerOfMass.position);
        rb.useGravity = false;
        rb.isKinematic = false;

        rb.sleepThreshold = 0f;

        rb.mass = mass;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

        if (startForceRelative != Vector3.zero)
        {
            rb.AddRelativeForce(startForceRelative, ForceMode.Impulse);
        }
    }


    void OnDrawGizmosSelected()
    {
        if (centerOfMass != null)
        {
            Gizmos.color = Color.red;
            float sphereRadius = 0.15f;
            Gizmos.DrawWireSphere(centerOfMass.position, sphereRadius);
        }
    }
}