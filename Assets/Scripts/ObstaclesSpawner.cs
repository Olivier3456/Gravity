using System.Collections;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField, Range(0f, 60f)] private float spawnRate = 5f;

    [SerializeField] private Transform sphereCenter;
    [SerializeField] private float spawnSphereRadius = 50f;
    [SerializeField] private Vector2 obstaclesSpeed = new(10f, 20f);


    private float spawnTimer;



    void Start()
    {
        if (obstaclePrefabs.Length == 0f)
        {
            Debug.LogWarning("No obstacles prefab in the array!");
            Destroy(this);
            return;
        }
    }


    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnRate)
        {
            return;
        }
        spawnTimer = 0f;

        Vector3 spawnPosition = sphereCenter.position + (Random.onUnitSphere * spawnSphereRadius);
        int index = Random.Range(0, obstaclePrefabs.Length - 1);
        if (obstaclePrefabs[index] == null)
        {
            Debug.LogWarning($"Object at index {index} is null!");
            return;
        }
        GameObject instantiatedObstacle = Instantiate(obstaclePrefabs[index], spawnPosition, Quaternion.identity, transform);

        float speed = (float)Random.Range(obstaclesSpeed.x, obstaclesSpeed.y);
        if (!instantiatedObstacle.TryGetComponent(out Rigidbody rigidbody))
        {
            return;
        }


        Vector3 directionTosphereCenter = (sphereCenter.position - instantiatedObstacle.transform.position).normalized;

        Vector3 randomVector3 = new((float)Random.Range(-1f, 1f),
                                    (float)Random.Range(-1f, 1f),
                                    (float)Random.Range(-1f, 1f));
        Vector3 randomDirection = randomVector3.normalized;
        float randomDirectionWeight = 0.25f;
        Vector3 finalDirection = (directionTosphereCenter + (randomDirection * randomDirectionWeight)).normalized;

        rigidbody.linearVelocity = finalDirection * speed;

        StartCoroutine(DestroyObstacleCoroutine(instantiatedObstacle, speed));
    }


    private IEnumerator DestroyObstacleCoroutine(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(target);
    }
}
