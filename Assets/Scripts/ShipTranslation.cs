using UnityEngine;

public class ShipTranslation : MonoBehaviour
{
    [SerializeField] private Ship ship;
    [SerializeField] private ShipFuelManager shipFuelManager;
    private IShipMovementInputs Inputs => ship.ShipInputs;
    [SerializeField] private float positionThrustersForce = 100f;

    // Linear velocity targeted at full stick deflection when the stabilizer is on, in m/s.
    // Doubles as the speed governor's cap when it is off, so both modes top out identically.
    [SerializeField] private float maxLinearVelocity = 10f;

    // Force applied per m/s of error, per kilogram of ship. 1 / fixedDeltaTime (50) converges
    // in one step (very sharp); a fifth of that gives a softer response.
    [SerializeField] private float responseGainPerKilogram = 10f;

    // Read live rather than cached in Start(): Ship.Start() assigns the Rigidbody mass, and the
    // execution order between the two Start() calls is undefined.
    private float ResponseGain => responseGainPerKilogram * ship.Rigidbody.mass;

    // Both modes share the same axis mapping and the same maximum force: PositionAxisX drives
    // local X (right), PositionAxisY drives local Y (up), PositionAxisZ drives local Z (forward).
    private Vector3 LocalInputs => new Vector3(
        Inputs.PositionAxisX,
        Inputs.PositionAxisY,
        Inputs.PositionAxisZ);

    // public Vector3 CurrentForce { get; private set; }

    private Vector3 currentForceNormalized;
    public Vector3 CurrentForceNormalized => currentForceNormalized;

    public float ThrustersForce => positionThrustersForce;


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
        if (!shipFuelManager.HasFuel)
        {
            return;
        }

        if (ship.IsAutoPositionStabilizerActive)
        {
            // Velocity command: the stick asks for a speed, released means stop.
            ApplyVelocityHoldForces();
        }
        else
        {
            // Thrust command: the stick asks for an acceleration, acquired velocity is kept.
            ApplyDirectThrustForces();
        }
    }


    private void ApplyDirectThrustForces()
    {
        Vector3 inputs = LocalInputs;
        Vector3 localVelocity = ship.Rigidbody.transform.InverseTransformDirection(ship.Rigidbody.linearVelocity);

        Vector3 force = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            // Speed governor: same cap as the stabilizer, but enforced by cutting the thruster
            // instead of braking, so this mode never applies a force the pilot did not command.
            // Thrust opposing the current velocity always goes through, so slowing down and
            // reversing stay possible above the cap.
            bool thrustingAwayFromZero = inputs[i] * localVelocity[i] > 0f;
            if (thrustingAwayFromZero && Mathf.Abs(localVelocity[i]) >= maxLinearVelocity)
            {
                continue;
            }

            force[i] = inputs[i] * positionThrustersForce;
            currentForceNormalized[i] = inputs[i];
        }

        // CurrentForce = force;

        ship.Rigidbody.AddRelativeForce(force, ForceMode.Force);
    }


    private void ApplyVelocityHoldForces()
    {
        Vector3 localVelocity = ship.Rigidbody.transform.InverseTransformDirection(ship.Rigidbody.linearVelocity);
        Vector3 targetLocalVelocity = LocalInputs * maxLinearVelocity;
        float gain = ResponseGain;

        Vector3 force = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            float deltaVelocity = targetLocalVelocity[i] - localVelocity[i];
            force[i] = Mathf.Clamp(deltaVelocity * gain, -positionThrustersForce, positionThrustersForce);
            currentForceNormalized[i] = force[i] / positionThrustersForce;
        }

        // CurrentForce = force;

        ship.Rigidbody.AddRelativeForce(force, ForceMode.Force);
    }
}
