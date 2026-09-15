using UnityEngine;

public class ShipAudio : MonoBehaviour
{
    [SerializeField] private Ship ship;
    [SerializeField] private ShipTranslation shipTranslation;
    [SerializeField] private ShipRotation shipRotation;
    [Space]
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
        // // Obviously the ship audio class should not have to verity the ship status. Instead, the ship should have a property with each of his thruster current usage status.
        // if (ship.IsCrashed)
        // {
        //     return;
        // }
        // if (ship.IsDocked)
        // {
        //     return;
        // }


        audioSourceRight.volume = Mathf.Clamp01(-shipTranslation.CurrentForceNormalized.x);
        audioSourceLeft.volume = Mathf.Clamp01(shipTranslation.CurrentForceNormalized.x);

        audioSourceUp.volume = Mathf.Clamp01(-shipTranslation.CurrentForceNormalized.y);
        audioSourceDown.volume = Mathf.Clamp01(shipTranslation.CurrentForceNormalized.y);

        audioSourceForward.volume = Mathf.Clamp01(-shipTranslation.CurrentForceNormalized.z);
        audioSourceBackward.volume = Mathf.Clamp01(shipTranslation.CurrentForceNormalized.z);

        audioSourceRotation.volume = Mathf.Max(
                                                Mathf.Abs(shipRotation.CurrentTorqueNormalized.x),
                                                Mathf.Abs(shipRotation.CurrentTorqueNormalized.y),
                                                Mathf.Abs(shipRotation.CurrentTorqueNormalized.z)
                                                );
    }
}
