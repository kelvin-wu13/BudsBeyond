using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Objects")]
    public GameObject initialBackground;
    public GameObject mainGameplayBackground;
    public PlatformSpawner platformSpawner;
    public BackgroundFollow backgroundFollower;

    [Header("Settings")]
    [Tooltip("How long to wait after launch before starting the game (in seconds).")]
    public float gameplayStartDelay = 1.5f;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        initialBackground.SetActive(true);
        mainGameplayBackground.SetActive(false);
        platformSpawner.enabled = false;
        backgroundFollower.enabled = false;
    }

    public void StartGameplay()
    {
        StartCoroutine(StartGameplayRoutine());
    }

    private IEnumerator StartGameplayRoutine()
    {
        yield return new WaitForSeconds(gameplayStartDelay);

        initialBackground.SetActive(false);
        mainGameplayBackground.SetActive(true);

        platformSpawner.ActivateSpawner();
        platformSpawner.enabled = true;

        backgroundFollower.enabled = true;
    }
}