// using System;
// using UnityEngine;

// public class ShipRotationStabilizer : MonoBehaviour
// {
//     [SerializeField] private Ship ship;

//     // Angular velocity targeted at full stick deflection, in rad/s.
//     [SerializeField] private float maxAngularVelocity = 1.5f;
//     // Torque applied per rad/s of error. inertia / fixedDeltaTime converges in one step (very sharp);
//     // divide by 3 or 4 for a softer response.
//     [SerializeField] private float responseGain = 50f;

//     private Rigidbody Rb => ship.Rigidbody;
//     private float RotationThrustersForce => ship.RotationThrustersForce;
//     private IShipMovementInputs ShipInputs => ship.ShipInputs;


//     void OnEnable()
//     {
//         ship.onForcesApplied += OnShipForcesApplied;
//     }
//     void OnDisable()
//     {
//         ship.onForcesApplied -= OnShipForcesApplied;
//     }

//     private void OnShipForcesApplied(object sender, EventArgs args)
//     {
//         AutoRotationStabilization();
//     }


//     // (Claude Code)
//     private void AutoRotationStabilization()
//     {
//         if (!ship.AutoRotationStabilizer)
//         {
//             return;
//         }

//         Vector3 localAngularVelocity = Rb.transform.InverseTransformDirection(Rb.angularVelocity);

//         // Axis mapping, see Ship.ApplyRotationForces: RotationAxisX drives local -X (pitch),
//         // RotationAxisY drives local -Z (roll), RotationAxisZ drives local -Y (yaw).
//         Vector3 targetAngularVelocity = maxAngularVelocity * new Vector3(
//             -ShipInputs.RotationAxisX,
//             -ShipInputs.RotationAxisZ,
//             -ShipInputs.RotationAxisY);

//         Vector3 torque = Vector3.zero;
//         for (int i = 0; i < 3; i++)
//         {
//             float deltaAngularVelocity = targetAngularVelocity[i] - localAngularVelocity[i];
//             torque[i] = Mathf.Clamp(deltaAngularVelocity * responseGain, -RotationThrustersForce, RotationThrustersForce);
//         }

//         Rb.AddRelativeTorque(torque);
//         // Debug.Log("Stabilization torque: " + torque);
//     }



//     // =============================
//     // Previous versions:
//     // =============================

//     // (Commented by Claude Code) Torque-based version: input asks for torque, stabilizer spends the remaining
//     // thruster capacity (1 - |input|) against the rotation. Superseded because both
//     // terms cancel out at half stick. Needs the early return in
//     // Ship.ApplyRotationForces to be removed to work again.
//     // private void AutoRotationStabilization()
//     // {
//     //     if (!ship.AutoRotationStabilizer)
//     //     {
//     //         return;
//     //     }
//     //
//     //     Vector3 stabilizationForce = Vector3.zero;
//     //     Vector3 localAngularVelocity = Rb.transform.InverseTransformDirection(Rb.angularVelocity);
//     //     Vector3 localAngularDirection = localAngularVelocity.normalized;
//     //
//     //     // Note: in Ship.ApplyRotationForces, RotationAxisY drives the local Z axis (roll)
//     //     // and RotationAxisZ drives the local Y axis (yaw), hence the Y/Z swap below.
//     //     float threshold = 0.000001f;
//     //     if (Mathf.Abs(localAngularVelocity.x) > threshold)
//     //     {
//     //         stabilizationForce += -1 * RotationThrustersForce * (1 - Mathf.Abs(ShipInputs.RotationAxisX)) * new Vector3(localAngularDirection.x, 0f, 0f);
//     //     }
//     //     if (Mathf.Abs(localAngularVelocity.y) > threshold)
//     //     {
//     //         stabilizationForce += -1 * RotationThrustersForce * (1 - Mathf.Abs(ShipInputs.RotationAxisZ)) * new Vector3(0f, localAngularDirection.y, 0f);
//     //     }
//     //     if (Mathf.Abs(localAngularVelocity.z) > threshold)
//     //     {
//     //         stabilizationForce += -1 * RotationThrustersForce * (1 - Mathf.Abs(ShipInputs.RotationAxisY)) * new Vector3(0f, 0f, localAngularDirection.z);
//     //     }
//     //     Rb.AddRelativeTorque(stabilizationForce);
//     //     // Debug.Log("Stabilization force: " + stabilizationForce);
//     // }

//     // private void AutoRotationStabilization()
//     // {
//     //     if (!ship.AutoRotationStabilizer)
//     //     {
//     //         return;
//     //     }

//     //     Vector3 stabilizationForce = Vector3.zero;
//     //     Vector3 localAngularVelocity = rb.transform.InverseTransformDirection(rb.angularVelocity);
//     //     Vector3 localAngularDirection = localAngularVelocity.normalized;

//     //     // X axis:
//     //     if (ShipInputs.RotationAxisX == 0f && Mathf.Abs(localAngularVelocity.x) > 0.000001f)
//     //     {
//     //         stabilizationForce += rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatusX) * -1 * new Vector3(localAngularDirection.x, 0f, 0f);
//     //         rotationStabilizationStatusX = Mathf.Clamp01(rotationStabilizationStatusX + (Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration));
//     //         Debug.Log("Stabilization X.");
//     //     }
//     //     else
//     //     {
//     //         rotationStabilizationStatusX = 0f;
//     //     }

//     //     // Y axis:
//     //     if (ShipInputs.RotationAxisY == 0f && Mathf.Abs(localAngularVelocity.y) > 0.000001f)
//     //     {
//     //         stabilizationForce += rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatusY) * -1 * new Vector3(0f, localAngularDirection.y, 0f);
//     //         rotationStabilizationStatusY = Mathf.Clamp01(rotationStabilizationStatusY + (Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration));
//     //         Debug.Log("Stabilization Y.");
//     //     }
//     //     else
//     //     {
//     //         rotationStabilizationStatusY = 0f;
//     //     }

//     //     // Z axis:
//     //     if (ShipInputs.RotationAxisZ == 0f && Mathf.Abs(localAngularVelocity.z) > 0.000001f)
//     //     {
//     //         stabilizationForce += rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatusZ) * -1 * new Vector3(0f, 0f, localAngularDirection.z);
//     //         rotationStabilizationStatusZ = Mathf.Clamp01(rotationStabilizationStatusZ + (Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration));
//     //         Debug.Log("Stabilization Z.");
//     //     }
//     //     else
//     //     {
//     //         rotationStabilizationStatusZ = 0f;
//     //     }

//     //     // Apply force to rigidbody:
//     //     if (stabilizationForce != Vector3.zero)
//     //     {
//     //         rb.AddRelativeTorque(stabilizationForce);
//     //     }
//     // }


//     // private void AutoRotationStabilization()
//     // {
//     //     if (!ship.AutoRotationStabilizer)
//     //     {
//     //         return;
//     //     }

//     //     if (ShipInputs.RotationAxisX == 0f &&
//     //         ShipInputs.RotationAxisY == 0f &&
//     //         ShipInputs.RotationAxisZ == 0f &&
//     //         rb.angularVelocity.magnitude > 0.000001f
//     //                 )
//     //     {
//     //         Vector3 localAngularVelocity = rb.transform.InverseTransformDirection(rb.angularVelocity);
//     //         Vector3 localAngularDirection = localAngularVelocity.normalized;
//     //         Vector3 stabilization = rotationThrustersForce * rotationStabilizationMagnitudeCurve.Evaluate(rotationStabilizationStatus) * -1 * localAngularDirection;
//     //         // Debug.Log($"Auto rotation correction! Current angular velocity: {localAngularVelocity}. Force applied: {stabilization.magnitude}.");
//     //         rotationStabilizationStatus += Time.fixedDeltaTime / rotationStabilizationMagnitudeCurveDuration;
//     //         rb.AddRelativeTorque(stabilization);
//     //     }
//     //     else
//     //     {
//     //         rotationStabilizationStatus = 0f;
//     //     }
//     // }
// }
