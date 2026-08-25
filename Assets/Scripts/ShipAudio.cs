using UnityEngine;

public class ShipAudio : MonoBehaviour
{
    [SerializeField] private Ship ship;
    [SerializeField] private AudioSource audioSourceRight;
    [SerializeField] private AudioSource audioSourceLeft;
    [SerializeField] private AudioSource audioSourceUp;
    [SerializeField] private AudioSource audioSourceDown;
    [SerializeField] private AudioSource audioSourceForward;
    [SerializeField] private AudioSource audioSourceBackward;

    [SerializeField, Space] private AudioSource audioSourceRotation;


    void Start()
    {
        audioSourceRight.Play();
        audioSourceLeft.Play();
        audioSourceUp.Play();
        audioSourceDown.Play();
        audioSourceForward.Play();
        audioSourceBackward.Play();
        audioSourceRotation.Play();
    }


    void Update()
    {
        // Obviously the ship audio class should not have to verity the ship status. Instead, the ship should have a property with each of his thruster current usage status.
        if (ship.IsCrashed)
        {
            return;
        }
        if (ship.IsDocked)
        {
            return;
        }


        audioSourceRight.volume = Mathf.Clamp01(ship.ShipInputs.PositionAxisX);
        audioSourceLeft.volume = Mathf.Clamp01(-ship.ShipInputs.PositionAxisX);

        audioSourceUp.volume = Mathf.Clamp01(ship.ShipInputs.PositionAxisY);
        audioSourceDown.volume = Mathf.Clamp01(-ship.ShipInputs.PositionAxisY);

        audioSourceForward.volume = Mathf.Clamp01(ship.ShipInputs.PositionAxisZ);
        audioSourceBackward.volume = Mathf.Clamp01(-ship.ShipInputs.PositionAxisZ);

        audioSourceRotation.volume = Mathf.Max(
                                                Mathf.Abs(ship.ShipInputs.RotationAxisX),
                                                Mathf.Abs(ship.ShipInputs.RotationAxisY),
                                                Mathf.Abs(ship.ShipInputs.RotationAxisZ)
                                                );
    }
}
