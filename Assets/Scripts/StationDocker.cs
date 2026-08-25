using UnityEngine;

public class StationDocker : MonoBehaviour, IDocker
{
    [SerializeField] private Transform rotationAnchor;
    public Transform RotationAnchor => rotationAnchor;
}
