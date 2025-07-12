using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Prefabs")]
    public List<GameObject> normalPlatformPrefabs;
    public List<GameObject> trapPlatformPrefabs;
    public List<GameObject> movingPlatformPrefabs;
    public List<GameObject> launchPlatformPrefabs;

    [Header("Spawner Settings")]
    public Transform playerTransform;
    public int startingPlatforms = 25;
    public float spawnOffset = 15f;

    [Header("Spawn Probabilities")]
    [Range(0, 1)] public float trapSpawnChance = 0.15f;
    [Range(0, 1)] public float movingPlatformChance = 0.10f;
    [Range(0, 1)] public float launchPlatformChance = 0.05f;

    [Header("Platform Positioning")]
    public float horizontalRange = 4f;
    public float minYSpacing = 1.5f;
    public float maxYSpacing = 4.0f;

    private float nextSpawnY;

    void Start()
    {
        if (normalPlatformPrefabs.Count > 0)
        {
            GameObject firstPlatform = Instantiate(normalPlatformPrefabs[0], transform.position, Quaternion.identity);
            ScoreManager.instance.RegisterPlatform(firstPlatform.transform);
            nextSpawnY = transform.position.y;
        }
        else
        {
            Debug.LogError("The 'Normal Platform Prefabs' list is empty! Cannot start the game.");
            return;
        }

        for (int i = 1; i < startingPlatforms; i++)
        {
            SpawnNextPlatform();
        }
    }

    void Update()
    {
        while (nextSpawnY < playerTransform.position.y + spawnOffset)
        {
            SpawnNextPlatform();
        }
    }

    void SpawnNextPlatform()
    {
        float randomX = Random.Range(-horizontalRange, horizontalRange);
        nextSpawnY += Random.Range(minYSpacing, maxYSpacing);
        Vector2 spawnPosition = new Vector2(randomX, nextSpawnY);
        GameObject platformToSpawn = ChoosePlatform();

        GameObject newPlatform = Instantiate(platformToSpawn, spawnPosition, Quaternion.identity);
        ScoreManager.instance.RegisterPlatform(newPlatform.transform);
    }

    GameObject ChoosePlatform()
    {
        float roll = Random.value;

        if (roll < movingPlatformChance && movingPlatformPrefabs.Count > 0)
            return movingPlatformPrefabs[Random.Range(0, movingPlatformPrefabs.Count)];
        else if (roll < movingPlatformChance + launchPlatformChance && launchPlatformPrefabs.Count > 0)
            return launchPlatformPrefabs[Random.Range(0, launchPlatformPrefabs.Count)];
        else if (roll < movingPlatformChance + launchPlatformChance + trapSpawnChance && trapPlatformPrefabs.Count > 0)
            return trapPlatformPrefabs[Random.Range(0, trapPlatformPrefabs.Count)];
        else
            return normalPlatformPrefabs[Random.Range(0, normalPlatformPrefabs.Count)];
    }
}