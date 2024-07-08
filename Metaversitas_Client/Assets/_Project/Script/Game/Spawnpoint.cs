using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    public static Spawnpoint Instance { get; private set; }

    public Collider spawnArea;

    // Function to get a random spawn point from the area
    public Transform GetRandomSpawnPoint()
    {
        if (spawnArea == null)
        {
            Debug.LogError("Spawn area collider not assigned!");
            return null;
        }

        Vector3 randomPoint = spawnArea.bounds.center +
                              new Vector3(
                                  Random.Range(-spawnArea.bounds.extents.x, spawnArea.bounds.extents.x),
                                  0,
                                  Random.Range(-spawnArea.bounds.extents.z, spawnArea.bounds.extents.z)
                              );

        // Create a new transform for the random point
        Transform spawnTransform = new GameObject("SpawnPoint").transform;
        spawnTransform.position = randomPoint;

        return spawnTransform;
    }
}
