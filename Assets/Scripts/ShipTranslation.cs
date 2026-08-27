using UnityEngine;

public class ShipTranslation : MonoBehaviour
{
    [SerializeField] private Ship ship;
    private IShipMovementInputs Inputs => ship.ShipInputs;
    [SerializeField] private float positionThrustersForce = 100f;


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

        ApplyPositionForces();
    }


    private void ApplyPositionForces()
    {
        ship.Rigidbody.AddRelativeForce(Vector3.up * Inputs.PositionAxisY * positionThrustersForce, ForceMode.Force);
        ship.Rigidbody.AddRelativeForce(Vector3.forward * Inputs.PositionAxisZ * positionThrustersForce, ForceMode.Force);
        ship.Rigidbody.AddRelativeForce(Vector3.right * Inputs.PositionAxisX * positionThrustersForce, ForceMode.Force);
    }
}
