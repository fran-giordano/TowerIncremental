using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject objectToSpawn; // The prefab you want to spawn
    public float spawnInterval = 2.0f; // Time between spawns (in seconds)

    private void Start()
    {
        // Start the automatic spawning loop when the game starts
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Wait for the specified interval time
            yield return new WaitForSeconds(spawnInterval);

            // Spawn the object at the current spawner's position and rotation
            Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
    }
}
