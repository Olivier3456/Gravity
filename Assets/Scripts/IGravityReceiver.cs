using UnityEngine;

public interface IGravityReceiver
{
    // Rigidbody rigidbody { get; }
    Vector3 GetPosition();
    float GetMass();
    void ApplyForce(Vector3 force);
}