using System;
using UnityEngine;
using UnityEngine.Events;

public class ShipDocker : MonoBehaviour, IDocker
{
    [SerializeField] private float dockingTimeThreshold = 5f;
    [SerializeField] private Ship ship;
    [SerializeField] private Transform rotationAnchor;

    [SerializeField] private float angleThreshold = 5f;

    public float DockingStatus { get; private set; }

    public Transform RotationAnchor => rotationAnchor;

    private IDocker otherDocker;

    [Space] public UnityEvent<float> onDockTimeStatusChanged;


    void OnTriggerEnter(Collider other)
    {
        otherDocker = other.transform.GetComponent<IDocker>();

        if (otherDocker == null)
        {
            return;
        }

        Debug.Log("Station Docker entered trigger.");
    }


    void Update()
    {
        if (ship.IsCrashed)
        {
            return;
        }
        if (ship.IsDocked)
        {
            return;
        }

        if (otherDocker != null)
        {
            float angle = Vector3.Angle(otherDocker.RotationAnchor.forward, -RotationAnchor.forward);
            if (angle > angleThreshold)
            {
                Debug.Log($"Not the good angle to dock! ({angle}).");
                return;
            }

            DockingStatus = Mathf.Clamp01(DockingStatus + (Time.deltaTime / dockingTimeThreshold));
            onDockTimeStatusChanged?.Invoke(DockingStatus);
            Debug.Log("New Docking Status: " + DockingStatus);

            if (DockingStatus == 1f)
            {
                ship.SetDocked(true);
                // shipDocker.SetDocked(true);
                Debug.Log("Ship docked successfully!");
            }
        }
        else
        {
            DockingStatus = 0f;
            onDockTimeStatusChanged?.Invoke(DockingStatus);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<IDocker>() == otherDocker)
        {
            otherDocker = null;
            Debug.Log("Station Docker exited trigger.");
        }
    }
}
