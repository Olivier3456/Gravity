using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    // [SerializeField] private float gravityInfluenceRadius = 1f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SphereCollider sphereTrigger;
    [SerializeField] private float mass = 1000f;

    private List<IGravityReceiver> currentGravityRecevers = new();

    private const float G = 10f;

    private float triggerRadiusMassFactor = 0.1f;
    private float TriggerRadius => mass * triggerRadiusMassFactor;


    void Start()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        sphereTrigger.isTrigger = true;
        sphereTrigger.center = transform.position;
        sphereTrigger.radius = TriggerRadius;
    }


    void FixedUpdate()
    {
        for (int i = 0; i < currentGravityRecevers.Count; i++)
        {
            if (currentGravityRecevers[i] == null)
            {
                Debug.LogError($"{typeof(IGravityReceiver)} is null!");
                continue;
            }

            Vector3 targetObjectPosition = currentGravityRecevers[i].GetPosition();
            Vector3 myPosition = transform.position;
            Vector3 toTargetObject = myPosition - targetObjectPosition;
            Vector3 direction = toTargetObject.normalized;
            float sqrDistance = toTargetObject.sqrMagnitude;
            float force = G * (currentGravityRecevers[i].GetMass() * mass) / sqrDistance;
            Vector3 forceVector = direction * force;

            currentGravityRecevers[i].ApplyForce(forceVector);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IGravityReceiver gravityRecever))
        {
            return;
        }

        if (!currentGravityRecevers.Contains(gravityRecever))
        {
            currentGravityRecevers.Add(gravityRecever);
        }

        Debug.Log($"Added a new {typeof(IGravityReceiver)} to the list. Current count: {currentGravityRecevers.Count}.");
    }
    void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out IGravityReceiver gravityRecever))
        {
            return;
        }

        if (currentGravityRecevers.Contains(gravityRecever))
        {
            currentGravityRecevers.Remove(gravityRecever);
        }

        Debug.Log($"Removed a {typeof(IGravityReceiver)} from the list. Current count: {currentGravityRecevers.Count}.");
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, TriggerRadius);
    }
}
