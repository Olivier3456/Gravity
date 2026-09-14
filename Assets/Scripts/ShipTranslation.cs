using UnityEngine;

public class ShipTranslation : MonoBehaviour
{
    [SerializeField] private Ship ship;
    private IShipMovementInputs Inputs => ship.ShipInputs;
    [SerializeField] private float positionThrustersForce = 100f;

    // Linear velocity targeted at full stick deflection, in m/s.
    [SerializeField] private float maxLinearVelocity = 10f;

    // Force applied per m/s of error. mass / fixedDeltaTime converges in one step (very sharp);
    // divide by 3 or 4 for a softer response.
    [SerializeField] private float responseGainPerTon = 15f;


    private float responseGain;


    private void Start()
    {
        responseGain = responseGainPerTon * ship.Rigidbody.mass;
    }


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

        if (ship.IsAutoPositionStabilizerActive)
        {
            AutoPositionStabilization();
        }
        else
        {
            ApplyPositionForces();
        }
    }


    private void ApplyPositionForces()
    {
        ship.Rigidbody.AddRelativeForce(Vector3.up * Inputs.PositionAxisY * positionThrustersForce, ForceMode.Force);
        ship.Rigidbody.AddRelativeForce(Vector3.forward * Inputs.PositionAxisZ * positionThrustersForce, ForceMode.Force);
        ship.Rigidbody.AddRelativeForce(Vector3.right * Inputs.PositionAxisX * positionThrustersForce, ForceMode.Force);
    }


    // (Claude)
    private void AutoPositionStabilization()
    {
        Vector3 localVelocity = ship.Rigidbody.transform.InverseTransformDirection(ship.Rigidbody.linearVelocity);

        // Axis mapping, see ApplyPositionForces: PositionAxisX drives local X (right),
        // PositionAxisY drives local Y (up), PositionAxisZ drives local Z (forward).
        Vector3 targetLocalVelocity = maxLinearVelocity * new Vector3(
            Inputs.PositionAxisX,
            Inputs.PositionAxisY,
            Inputs.PositionAxisZ);

        Vector3 force = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            float deltaVelocity = targetLocalVelocity[i] - localVelocity[i];
            force[i] = Mathf.Clamp(deltaVelocity * responseGain, -positionThrustersForce, positionThrustersForce);
        }

        ship.Rigidbody.AddRelativeForce(force, ForceMode.Force);
    }
}
