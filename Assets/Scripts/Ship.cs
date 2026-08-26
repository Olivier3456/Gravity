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
    // private float rotationStabilizationStatus;
    private float rotationStabilizationStatusX;
    private float rotationStabilizationStatusY;
    private float rotationStabilizationStatusZ;


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
        AutoRotationStabilization();
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
    }


    private void AutoRotationStabilization()
    {
        if (!autoRotationStabilizer)
        {
            return;
        }

        Vector3 localAngularVelocity = rb.transform.InverseTransformDirection(rb.angularVelocity);
        Vector3 localAngularDirection = localAngularVelocity.normalized;

        Vector3 stabilizationForce = Vector3.zero;

        // X axis:
        if (ShipInputs.RotationAxisX == 0f && Mathf.Abs(localAngularVelocity.x) > 0.000001f)
        {
            stabilizationForce += rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatusX) * -1 * new Vector3(localAngularDirection.x, 0f, 0f);
            rotationStabilizationStatusX = Mathf.Clamp01(rotationStabilizationStatusX + (Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration));
            Debug.Log("Stabilization X.");
        }
        else
        {
            rotationStabilizationStatusX = 0f;
        }

        // Y axis:
        if (ShipInputs.RotationAxisY == 0f && Mathf.Abs(localAngularVelocity.y) > 0.000001f)
        {
            stabilizationForce += rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatusY) * -1 * new Vector3(0f, localAngularDirection.y, 0f);
            rotationStabilizationStatusY = Mathf.Clamp01(rotationStabilizationStatusY + (Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration));
            Debug.Log("Stabilization Y.");
        }
        else
        {
            rotationStabilizationStatusY = 0f;
        }

        // Z axis:
        if (ShipInputs.RotationAxisZ == 0f && Mathf.Abs(localAngularVelocity.z) > 0.000001f)
        {
            stabilizationForce += rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatusZ) * -1 * new Vector3(0f, 0f, localAngularDirection.z);
            rotationStabilizationStatusZ = Mathf.Clamp01(rotationStabilizationStatusZ + (Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration));
            Debug.Log("Stabilization Z.");
        }
        else
        {
            rotationStabilizationStatusZ = 0f;
        }

        // Apply force to rigidbody:
        if (stabilizationForce != Vector3.zero)
        {
            rb.AddRelativeTorque(stabilizationForce);
        }



        // if (ShipInputs.RotationAxisX == 0f &&
        //     ShipInputs.RotationAxisY == 0f &&
        //     ShipInputs.RotationAxisZ == 0f &&
        //     rb.angularVelocity.magnitude > 0.000001f
        //             )
        // {
        //     Vector3 localAngularVelocity = rb.transform.InverseTransformDirection(rb.angularVelocity);
        //     Vector3 localAngularDirection = localAngularVelocity.normalized;
        //     Vector3 stabilization = rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatus) * -1 * localAngularDirection;
        //     // Debug.Log($"Auto rotation correction! Current angular velocity: {localAngularVelocity}. Force applied: {stabilization.magnitude}.");
        //     rotationStabilizationStatus += Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration;
        //     rb.AddRelativeTorque(stabilization);
        // }
        // else
        // {
        //     rotationStabilizationStatus = 0f;
        // }
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