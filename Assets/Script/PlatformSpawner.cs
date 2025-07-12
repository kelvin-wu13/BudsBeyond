using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Prefabs")]
    public List<GameObject> normalPlatformPrefabs;
    public List<GameObject> trapPlatformPrefabs;
    public List<GameObject> movingPlatformPrefabs;
    public List<GameObject> launchPlatformPrefabs;

    [Header("Player Movement Settings")]
    public float maxPlayerJumpHeight = 4.0f;
    public float maxPlayerJumpWidth = 3.5f;

    [Header("Spawner Settings")]
    public Transform playerTransform;
    public int startingPlatformRows = 10;
    public float spawnOffset = 20f;
    public int maxPlatformsPerRow = 3;

    [Header("Spawn Probabilities")]
    [Range(0, 1)] public float trapSpawnChance = 0.1f;
    [Range(0, 1)] public float movingPlatformChance = 0.15f;
    [Range(0, 1)] public float launchPlatformChance = 0.2f;

    [Header("Power-up & Coin Settings")]
    public GameObject coinPrefab;
    [Range(0, 1)] public float coinSpawnChance = 0.3f;
    [Range(0, 1)] public float powerUpSpawnChance = 0.15f;
    public float objectYOffset = 0.8f;

    [Header("Positioning & Clearance")]
    public float horizontalRange = 4f;
    public float minYSpacing = 1.0f;
    public float maxYSpacing = 3.0f;
    public float platformWidth = 2.0f;

    // --- Private Variables ---
    private Vector2 lastSafePlatformPosition;
    private float nextRowY;
    private bool didLastRowContainSpecial = false;
    private float powerUpCooldownEndTime = 0f; // <-- NEW: Tracks when the power-up cooldown ends.

    void Start()
    {
        lastSafePlatformPosition = transform.position;
        GameObject firstPlatform = Instantiate(normalPlatformPrefabs[0], transform.position, Quaternion.identity);
        TrySpawnObjectOnPlatform(firstPlatform);
        nextRowY = transform.position.y;

        for (int i = 0; i < startingPlatformRows; i++)
        {
            SpawnPlatformRow();
        }
    }

    void Update()
    {
        if (lastSafePlatformPosition.y < playerTransform.position.y + spawnOffset)
        {
            SpawnPlatformRow();
        }
    }

    void SpawnPlatformRow()
    {
        // This method remains the same as before.
        nextRowY += Random.Range(minYSpacing, maxYSpacing);
        int platformsToSpawn = Random.Range(1, maxPlatformsPerRow + 1);
        var spawnedPositionsInRow = new List<Vector2>();
        bool specialWasSpawnedThisRow = false;

        if (didLastRowContainSpecial)
        {
            for (int i = 0; i < platformsToSpawn; i++)
            {
                SpawnPlatform(true, ref spawnedPositionsInRow);
            }
            didLastRowContainSpecial = false;
        }
        else
        {
            SpawnPlatform(true, ref spawnedPositionsInRow);

            for (int i = 1; i < platformsToSpawn; i++)
            {
                bool wasSpecial = SpawnPlatform(false, ref spawnedPositionsInRow);
                if (wasSpecial)
                {
                    specialWasSpawnedThisRow = true;
                }
            }
            didLastRowContainSpecial = specialWasSpawnedThisRow;
        }
    }

    bool SpawnPlatform(bool forceNormal, ref List<Vector2> spawnedPositionsInRow)
    {
        // This method remains the same as before.
        int attempts = 0;
        while (attempts < 10)
        {
            float spawnX = (spawnedPositionsInRow.Count == 0)
                ? Mathf.Clamp(lastSafePlatformPosition.x + Random.Range(-maxPlayerJumpWidth, maxPlayerJumpWidth), -horizontalRange, horizontalRange)
                : Random.Range(-horizontalRange, horizontalRange);

            Vector2 potentialPosition = new Vector2(spawnX, nextRowY);

            if (IsPositionSafe(potentialPosition, spawnedPositionsInRow))
            {
                var (prefab, isSpecial) = ChoosePlatform(forceNormal);
                GameObject newPlatform = Instantiate(prefab, potentialPosition, Quaternion.identity);
                TrySpawnObjectOnPlatform(newPlatform);

                spawnedPositionsInRow.Add(potentialPosition);

                if (spawnedPositionsInRow.Count == 1)
                {
                    lastSafePlatformPosition = potentialPosition;
                }
                return isSpecial;
            }
            attempts++;
        }
        return false;
    }

    // --- UPDATED METHOD: Now includes a cooldown check for power-ups ---
    void TrySpawnObjectOnPlatform(GameObject platform)
    {
        if (platform.CompareTag("TrapPlatform")) return;

        float roll = Random.value;

        // Check for a power-up first, but only if the cooldown has expired.
        if (roll < powerUpSpawnChance && Time.time >= powerUpCooldownEndTime)
        {
            PlayerController player = playerTransform.GetComponent<PlayerController>();
            if (player != null)
            {
                CharacterData data = player.GetCharacterData();
                if (data != null && data.powerUpPrefab != null)
                {
                    Vector3 spawnPos = platform.transform.position + new Vector3(0, objectYOffset, 0);
                    Instantiate(data.powerUpPrefab, spawnPos, Quaternion.identity);

                    // --- NEW: Set the cooldown for the next power-up spawn ---
                    float cooldownDuration = Random.Range(5f, 7f);
                    powerUpCooldownEndTime = Time.time + cooldownDuration;
                }
            }
        }
        // If a power-up didn't spawn (either by chance or cooldown), check for a coin.
        else if (roll < powerUpSpawnChance + coinSpawnChance)
        {
            if (coinPrefab != null)
            {
                Vector3 spawnPos = platform.transform.position + new Vector3(0, objectYOffset, 0);
                Instantiate(coinPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    (GameObject prefab, bool isSpecial) ChoosePlatform(bool forceNormal)
    {
        // This method remains the same as before.
        if (forceNormal)
        {
            return (normalPlatformPrefabs[Random.Range(0, normalPlatformPrefabs.Count)], false);
        }

        float roll = Random.value;
        if (roll < movingPlatformChance) return (movingPlatformPrefabs[Random.Range(0, movingPlatformPrefabs.Count)], true);
        if (roll < movingPlatformChance + launchPlatformChance) return (launchPlatformPrefabs[Random.Range(0, launchPlatformPrefabs.Count)], true);
        if (roll < movingPlatformChance + launchPlatformChance + trapSpawnChance) return (trapPlatformPrefabs[Random.Range(0, trapPlatformPrefabs.Count)], true);

        return (normalPlatformPrefabs[Random.Range(0, normalPlatformPrefabs.Count)], false);
    }

    bool IsPositionSafe(Vector2 position, List<Vector2> otherPositions)
    {
        // This method remains the same as before.
        foreach (var pos in otherPositions)
        {
            if (Mathf.Abs(position.x - pos.x) < platformWidth)
            {
                return false;
            }
        }
        return true;
    }
}