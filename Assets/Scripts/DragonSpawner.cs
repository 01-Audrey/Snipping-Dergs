using UnityEngine;
using System.Collections;

public class DragonSpawner : MonoBehaviour
{
    [Header("Dragon Prefabs")]
    public GameObject dragonGreenPrefab;
    public GameObject dragonRedPrefab;
    public GameObject dragonWhitePrefab;

    [Header("Spawn Settings")]
    public float spawnDelay = 1f;
    public float spawnXOffset = 8f;
    public float spawnY = 0f;

    private bool isSpawning = false;

    public void StartSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnLoop()
    {
        while (isSpawning)
        {
            SpawnDragon();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnDragon()
    {
        int rand = Random.Range(0, 3);
        GameObject prefabToSpawn = null;

        if (rand == 0 && dragonGreenPrefab != null)
            prefabToSpawn = dragonGreenPrefab;
        else if (rand == 1 && dragonRedPrefab != null)
            prefabToSpawn = dragonRedPrefab;
        else if (dragonWhitePrefab != null)
            prefabToSpawn = dragonWhitePrefab;
        else
            prefabToSpawn = dragonRedPrefab;

        if (prefabToSpawn == null)
        {
            Debug.LogError("DragonSpawner: Missing prefab!");
            return;
        }

        float side = Random.value > 0.5f ? 1f : -1f;
        float spawnX = side * spawnXOffset;
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}