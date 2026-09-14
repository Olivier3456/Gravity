using UnityEngine;

public class ShipRotation : MonoBehaviour
{
    [SerializeField] private Ship ship;
    private IShipMovementInputs Inputs => ship.ShipInputs;
    [SerializeField] private float rotationThrustersForce = 50f;

    // Angular velocity targeted at full stick deflection when the stabilizer is on, in rad/s.
    // Doubles as the rate governor's cap when it is off, so both modes top out identically.
    [SerializeField] private float maxAngularVelocity = 1.5f;

    // Torque applied per rad/s of error. inertia / fixedDeltaTime converges in one step (very sharp);
    // divide by 3 or 4 for a softer response.
    [SerializeField] private float responseGain = 50f;

    // Both modes share the same axis mapping and the same maximum torque: RotationAxisX drives
    // local -X (pitch), RotationAxisY drives local -Z (roll), RotationAxisZ drives local -Y (yaw).
    private Vector3 LocalInputs => new Vector3(
        -Inputs.RotationAxisX,
        -Inputs.RotationAxisZ,
        -Inputs.RotationAxisY);

    public Vector3 CurrentTorque { get; private set; }

    public float ThrustersForce => rotationThrustersForce;


    void FixedUpdate()
    {
        if (ship.IsCrashed)
        {
            return;
        }
        if (ship.IsDocked)
        {
            return;
        }

        if (ship.IsAutoRotationStabilizerActive)
        {
            AutoRotationStabilization();
        }
        else
        {
            ApplyRotationForces();
        }
    }


    private void ApplyRotationForces()
    {
        Vector3 inputs = LocalInputs;
        Vector3 localAngularVelocity = ship.Rigidbody.transform.InverseTransformDirection(ship.Rigidbody.angularVelocity);

        Vector3 torque = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            // Rate governor: same cap as the stabilizer, but enforced by cutting the thruster
            // instead of braking, so this mode never applies a torque the pilot did not command.
            // Torque opposing the current rotation always goes through, so slowing down and
            // reversing stay possible above the cap.
            bool spinningUpAwayFromZero = inputs[i] * localAngularVelocity[i] > 0f;
            if (spinningUpAwayFromZero && Mathf.Abs(localAngularVelocity[i]) >= maxAngularVelocity)
            {
                continue;
            }

            torque[i] = inputs[i] * rotationThrustersForce;
        }

        CurrentTorque = torque;

        ship.Rigidbody.AddRelativeTorque(torque, ForceMode.Force);
    }


    private void AutoRotationStabilization()
    {
        Vector3 localAngularVelocity = ship.Rigidbody.transform.InverseTransformDirection(ship.Rigidbody.angularVelocity);
        Vector3 targetAngularVelocity = LocalInputs * maxAngularVelocity;

        Vector3 torque = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            float deltaAngularVelocity = targetAngularVelocity[i] - localAngularVelocity[i];
            torque[i] = Mathf.Clamp(deltaAngularVelocity * responseGain, -rotationThrustersForce, rotationThrustersForce);
        }

        CurrentTorque = torque;

        ship.Rigidbody.AddRelativeTorque(torque, ForceMode.Force);
    }



    // =============================
    // Previous versions:
    // =============================

    // (Commented by Claude Code) Torque-based version: input asks for torque, stabilizer spends the remaining
    // thruster capacity (1 - |input|) against the rotation. Superseded because both
    // terms cancel out at half stick. Needs the early return in
    // Ship.ApplyRotationForces to be removed to work again.
    // private void AutoRotationStabilization()
    // {
    //     if (!ship.AutoRotationStabilizer)
    //     {
    //         return;
    //     }
    //
    //     Vector3 stabilizationForce = Vector3.zero;
    //     Vector3 localAngularVelocity = Rb.transform.InverseTransformDirection(Rb.angularVelocity);
    //     Vector3 localAngularDirection = localAngularVelocity.normalized;
    //
    //     // Note: in Ship.ApplyRotationForces, RotationAxisY drives the local Z axis (roll)
    //     // and RotationAxisZ drives the local Y axis (yaw), hence the Y/Z swap below.
    //     float threshold = 0.000001f;
    //     if (Mathf.Abs(localAngularVelocity.x) > threshold)
    //     {
    //         stabilizationForce += -1 * RotationThrustersForce * (1 - Mathf.Abs(ShipInputs.RotationAxisX)) * new Vector3(localAngularDirection.x, 0f, 0f);
    //     }
    //     if (Mathf.Abs(localAngularVelocity.y) > threshold)
    //     {
    //         stabilizationForce += -1 * RotationThrustersForce * (1 - Mathf.Abs(ShipInputs.RotationAxisZ)) * new Vector3(0f, localAngularDirection.y, 0f);
    //     }
    //     if (Mathf.Abs(localAngularVelocity.z) > threshold)
    //     {
    //         stabilizationForce += -1 * RotationThrustersForce * (1 - Mathf.Abs(ShipInputs.RotationAxisY)) * new Vector3(0f, 0f, localAngularDirection.z);
    //     }
    //     Rb.AddRelativeTorque(stabilizationForce);
    //     // Debug.Log("Stabilization force: " + stabilizationForce);
    // }

    // private void AutoRotationStabilization()
    // {
    //     if (!ship.AutoRotationStabilizer)
    //     {
    //         return;
    //     }

    //     Vector3 stabilizationForce = Vector3.zero;
    //     Vector3 localAngularVelocity = rb.transform.InverseTransformDirection(rb.angularVelocity);
    //     Vector3 localAngularDirection = localAngularVelocity.normalized;

    //     // X axis:
    //     if (ShipInputs.RotationAxisX == 0f && Mathf.Abs(localAngularVelocity.x) > 0.000001f)
    //     {
    //         stabilizationForce += rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatusX) * -1 * new Vector3(localAngularDirection.x, 0f, 0f);
    //         rotationStabilizationStatusX = Mathf.Clamp01(rotationStabilizationStatusX + (Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration));
    //         Debug.Log("Stabilization X.");
    //     }
    //     else
    //     {
    //         rotationStabilizationStatusX = 0f;
    //     }

    //     // Y axis:
    //     if (ShipInputs.RotationAxisY == 0f && Mathf.Abs(localAngularVelocity.y) > 0.000001f)
    //     {
    //         stabilizationForce += rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatusY) * -1 * new Vector3(0f, localAngularDirection.y, 0f);
    //         rotationStabilizationStatusY = Mathf.Clamp01(rotationStabilizationStatusY + (Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration));
    //         Debug.Log("Stabilization Y.");
    //     }
    //     else
    //     {
    //         rotationStabilizationStatusY = 0f;
    //     }

    //     // Z axis:
    //     if (ShipInputs.RotationAxisZ == 0f && Mathf.Abs(localAngularVelocity.z) > 0.000001f)
    //     {
    //         stabilizationForce += rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatusZ) * -1 * new Vector3(0f, 0f, localAngularDirection.z);
    //         rotationStabilizationStatusZ = Mathf.Clamp01(rotationStabilizationStatusZ + (Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration));
    //         Debug.Log("Stabilization Z.");
    //     }
    //     else
    //     {
    //         rotationStabilizationStatusZ = 0f;
    //     }

    //     // Apply force to rigidbody:
    //     if (stabilizationForce != Vector3.zero)
    //     {
    //         rb.AddRelativeTorque(stabilizationForce);
    //     }
    // }


    // private void AutoRotationStabilization()
    // {
    //     if (!ship.AutoRotationStabilizer)
    //     {
    //         return;
    //     }

    //     if (ShipInputs.RotationAxisX == 0f &&
    //         ShipInputs.RotationAxisY == 0f &&
    //         ShipInputs.RotationAxisZ == 0f &&
    //         rb.angularVelocity.magnitude > 0.000001f
    //                 )
    //     {
    //         Vector3 localAngularVelocity = rb.transform.InverseTransformDirection(rb.angularVelocity);
    //         Vector3 localAngularDirection = localAngularVelocity.normalized;
    //         Vector3 stabilization = rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatus) * -1 * localAngularDirection;
    //         // Debug.Log($"Auto rotation correction! Current angular velocity: {localAngularVelocity}. Force applied: {stabilization.magnitude}.");
    //         rotationStabilizationStatus += Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration;
    //         rb.AddRelativeTorque(stabilization);
    //     }
    //     else
    //     {
    //         rotationStabilizationStatus = 0f;
    //     }
    // }
}
