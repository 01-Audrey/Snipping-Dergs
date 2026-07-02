using UnityEngine;
using System.Collections;

// ============================================================
// DragonSpawner.cs
// Spawns one dragon at a time, randomly picks from 3 types:
//   Green - regular, 1 hit
//   White - fast, 1 hit
//   Red   - slow tank, 3 hits
// ============================================================

public class DragonSpawner : MonoBehaviour
{
    [Header("Dragon Prefabs")]
    public GameObject dragonGreenPrefab;
    public GameObject dragonRedPrefab;
    public GameObject dragonWhitePrefab;

    [Header("Spawn Settings")]
    public float spawnDelay = 2f;
    public float spawnXOffset = 10f;
    public float spawnYRange = 1.5f;

    private bool isSpawning = false;
    private bool waitingForDragon = false;

    public void StartSpawning()
    {
        isSpawning = true;
        waitingForDragon = false;
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
            if (!waitingForDragon)
            {
                yield return new WaitForSeconds(spawnDelay);
                SpawnDragon();
                waitingForDragon = true;
            }
            yield return null;
        }
    }

    private void SpawnDragon()
    {
        // Pick random dragon type
        int rand = Random.Range(0, 3);
        GameObject prefabToSpawn = null;

        if (rand == 0 && dragonGreenPrefab != null)
            prefabToSpawn = dragonGreenPrefab;
        else if (rand == 1 && dragonRedPrefab != null)
            prefabToSpawn = dragonRedPrefab;
        else if (dragonWhitePrefab != null)
            prefabToSpawn = dragonWhitePrefab;

        if (prefabToSpawn == null)
        {
            Debug.LogError("DragonSpawner: Missing prefab!");
            return;
        }

        // Random side
        float side = Random.value > 0.5f ? 1f : -1f;
        float spawnX = side * spawnXOffset;
        float spawnY = Random.Range(-spawnYRange, spawnYRange);

        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);
        GameObject dragon = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        // Listen for done event
        DragonHealth health = dragon.GetComponent<DragonHealth>();
        if (health != null)
            health.OnDragonDone += OnDragonDone;
    }

    private void OnDragonDone()
    {
        waitingForDragon = false;
    }
}
