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

    private Vector2 lastSafePlatformPosition;
    private float nextRowY;
    private bool didLastRowContainSpecial = false;
    private float powerUpCooldownEndTime = 0f;

    void Start()
    {

    }

    public void ActivateSpawner()
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

    void TrySpawnObjectOnPlatform(GameObject platform)
    {
        if (platform.CompareTag("TrapPlatform")) return;

        float roll = Random.value;
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

                }
            }
        }
        else if (roll < powerUpSpawnChance + coinSpawnChance)
        {
            if (coinPrefab != null)
            {
                Vector3 spawnPos = platform.transform.position + new Vector3(0, objectYOffset, 0);
                Instantiate(coinPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    private void StartPowerUpCooldown()
    {
        float cooldownDuration = Random.Range(5f, 7f);
        powerUpCooldownEndTime = Time.time + cooldownDuration;
        Debug.Log($"Power-up collected! Cooldown started for {cooldownDuration:F1} seconds.");
    }

    void SpawnPlatformRow()
    {
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
            bool wasFirstPlatformSpecial = SpawnPlatform(false, ref spawnedPositionsInRow);
            if (wasFirstPlatformSpecial)
            {
                specialWasSpawnedThisRow = true;
            }

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

                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.RegisterPlatform(newPlatform.transform);
                }
                else
                {
                    Debug.LogError("ScoreManager instance is MISSING. Cannot register platform for scoring!");
                }

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

    (GameObject prefab, bool isSpecial) ChoosePlatform(bool forceNormal)
    {
        if (forceNormal)
        {
            return (normalPlatformPrefabs[Random.Range(0, normalPlatformPrefabs.Count)], false);
        }

        float roll = Random.value;
        float cumulativeProbability = 0f;

        cumulativeProbability += trapSpawnChance;
        if (roll < cumulativeProbability && trapPlatformPrefabs.Count > 0)
        {
            return (trapPlatformPrefabs[Random.Range(0, trapPlatformPrefabs.Count)], true);
        }

        cumulativeProbability += movingPlatformChance;
        if (roll < cumulativeProbability && movingPlatformPrefabs.Count > 0)
        {
            return (movingPlatformPrefabs[Random.Range(0, movingPlatformPrefabs.Count)], true);
        }

        cumulativeProbability += launchPlatformChance;
        if (roll < cumulativeProbability && launchPlatformPrefabs.Count > 0)
        {
            return (launchPlatformPrefabs[Random.Range(0, launchPlatformPrefabs.Count)], true);
        }

        return (normalPlatformPrefabs[Random.Range(0, normalPlatformPrefabs.Count)], false);
    }

    bool IsPositionSafe(Vector2 position, List<Vector2> otherPositions)
    {
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