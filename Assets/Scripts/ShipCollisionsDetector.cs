using System;
using UnityEngine;
using UnityEngine.Events;

public class ShipCollisionsDetector : MonoBehaviour
{
    [SerializeField] private float maxLinearSpeed = 0.1f;
    [SerializeField] private Ship ship;

    // public event EventHandler<float> onCrash;


    void OnCollisionEnter(Collision collision)
    {
        float collsionRetiveVelocityMagnitude = collision.relativeVelocity.magnitude;

        Debug.Log($"Collision! Relative speed: {collsionRetiveVelocityMagnitude}.");

        if (collsionRetiveVelocityMagnitude > maxLinearSpeed)
        {
            // onCrash?.Invoke(this, collsionRetiveVelocityMagnitude);
            ship.Crash();
        }
    }
}
