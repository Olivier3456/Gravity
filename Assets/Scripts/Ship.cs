using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float mass = 1f;

    [SerializeField] private Vector3 startForceRelative;

    [SerializeField] private float positionThrustersForce = 1f;
    [SerializeField] private float rotationThrustersForce = 1f;

    [SerializeField, Space(20)] private ShipKeyboardInputs shipKeyboardInputs;
    [SerializeField] private ShipGamepadInputs shipGamepadInputs;

    [SerializeField, Space(15)] private bool autoRotationStabilizer;
    [SerializeField] private float maxRotationStabilizationMagnitude = 100f;
    [SerializeField] private AnimationCurve rotationStabilizationMagnitudeCurve;
    [SerializeField] private float rotationStabilizationMagnitudeCurveDuration = 1f;
    private float rotationStabilizationStatus;


    public enum InputSource { Keyboard, Gamepad };
    [SerializeField, Space(20)] private InputSource inputSource;

    public IShipMovementInputs ShipInputs { get; private set; }


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
        }
        else
        {
            ShipInputs = shipGamepadInputs;
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


    void FixedUpdate()
    {
        if (IsCrashed)
        {
            return;
        }
        if (IsDocked)
        {
            return;
        }

        ApplyPositionForces();
        ApplyRotationForces();
    }


    private void ApplyPositionForces()
    {
        rb.AddRelativeForce(Vector3.up * ShipInputs.PositionAxisY * positionThrustersForce, ForceMode.Force);
        rb.AddRelativeForce(Vector3.forward * ShipInputs.PositionAxisZ * positionThrustersForce, ForceMode.Force);
        rb.AddRelativeForce(Vector3.right * ShipInputs.PositionAxisX * positionThrustersForce, ForceMode.Force);
    }


    private void ApplyRotationForces()
    {
        rb.AddRelativeTorque(Vector3.left * ShipInputs.RotationAxisX * rotationThrustersForce, ForceMode.Force);
        rb.AddRelativeTorque(Vector3.back * ShipInputs.RotationAxisY * rotationThrustersForce, ForceMode.Force);
        rb.AddRelativeTorque(Vector3.down * ShipInputs.RotationAxisZ * rotationThrustersForce, ForceMode.Force);

        if (autoRotationStabilizer &&
            ShipInputs.RotationAxisX == 0f &&
            ShipInputs.RotationAxisY == 0f &&
            ShipInputs.RotationAxisZ == 0f &&
            rb.angularVelocity.magnitude > 0.000001f
            )
        {
            Vector3 localAngularVelocity = rb.transform.InverseTransformDirection(rb.angularVelocity);
            Vector3 localAngularDirection = localAngularVelocity.normalized;
            Vector3 stabilization = rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatus) * -1 * localAngularDirection;
            // Debug.Log($"Auto rotation correction! Current angular velocity: {localAngularVelocity}. Force applied: {stabilization.magnitude}.");
            rotationStabilizationStatus += Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration;
            rb.AddRelativeTorque(stabilization);
        }
        else
        {
            rotationStabilizationStatus = 0f;
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