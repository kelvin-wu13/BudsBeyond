using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Settings")]
    public GameObject cloudPrefab;
    public List<Sprite> cloudSprites;

    [Header("Spawn Timing")]
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 7f;

    [Header("Spawn Position")]
    [Tooltip("The vertical area around the camera where clouds can spawn.")]
    public float verticalSpawnRange = 10f;

    [Header("Cloud Properties")]
    public float minSpeed = 0.5f;
    public float maxSpeed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.5f;

    private Camera mainCamera;
    private float screenWidthInWorldUnits;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnCloudRoutine());
    }

    private IEnumerator SpawnCloudRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);


            screenWidthInWorldUnits = mainCamera.orthographicSize * Screen.width / Screen.height;
            float spawnX = screenWidthInWorldUnits + 3f; 
            if (Random.value < 0.5f) spawnX = -spawnX;

            float spawnY = mainCamera.transform.position.y + Random.Range(-verticalSpawnRange, verticalSpawnRange);

            GameObject newCloud = Instantiate(cloudPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);

            newCloud.GetComponent<SpriteRenderer>().sprite = cloudSprites[Random.Range(0, cloudSprites.Count)];

            float scale = Random.Range(minScale, maxScale);
            newCloud.transform.localScale = new Vector3(scale, scale, 1);

            newCloud.GetComponent<Cloud>().speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}