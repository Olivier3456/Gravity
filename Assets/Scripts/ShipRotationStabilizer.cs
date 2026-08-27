using System;
using UnityEngine;

public class ShipRotationStabilizer : MonoBehaviour
{
    [SerializeField] private Ship ship;

    private Rigidbody Rb => ship.Rigidbody;
    private float RotationThrustersForce => ship.RotationThrustersForce;
    private IShipMovementInputs ShipInputs => ship.ShipInputs;


    void OnEnable()
    {
        ship.onForcesApplied += OnShipForcesApplied;
    }
    void OnDisable()
    {
        ship.onForcesApplied -= OnShipForcesApplied;
    }

    private void OnShipForcesApplied(object sender, EventArgs args)
    {
        AutoRotationStabilization();
    }

    private void AutoRotationStabilization()
    {
        if (!ship.AutoRotationStabilizer)
        {
            return;
        }

        Vector3 stabilizationForce = Vector3.zero;
        Vector3 localAngularVelocity = Rb.transform.InverseTransformDirection(Rb.angularVelocity);
        Vector3 localAngularDirection = localAngularVelocity.normalized;

        // Note: in Ship.ApplyRotationForces, RotationAxisY drives the local Z axis (roll)
        // and RotationAxisZ drives the local Y axis (yaw), hence the Y/Z swap below.
        float threshold = 0.000001f;
        if (Mathf.Abs(localAngularVelocity.x) > threshold)
        {
            stabilizationForce += -1 * RotationThrustersForce * (1 - Mathf.Abs(ShipInputs.RotationAxisX)) * new Vector3(localAngularDirection.x, 0f, 0f);
        }
        if (Mathf.Abs(localAngularVelocity.y) > threshold)
        {
            stabilizationForce += -1 * RotationThrustersForce * (1 - Mathf.Abs(ShipInputs.RotationAxisZ)) * new Vector3(0f, localAngularDirection.y, 0f);
        }
        if (Mathf.Abs(localAngularVelocity.z) > threshold)
        {
            stabilizationForce += -1 * RotationThrustersForce * (1 - Mathf.Abs(ShipInputs.RotationAxisY)) * new Vector3(0f, 0f, localAngularDirection.z);
        }
        Rb.AddRelativeTorque(stabilizationForce);
        // Debug.Log("Stabilization force: " + stabilizationForce);
    }

    // =============================
    // Previous versions:
    // =============================

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
