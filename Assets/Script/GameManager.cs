using System.Collections; // Required for Coroutines
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
        // Initial scene state remains the same
        initialBackground.SetActive(true);
        mainGameplayBackground.SetActive(false);
        platformSpawner.enabled = false;
        backgroundFollower.enabled = false;
    }

    // This function is still called by the LaunchPad instantly
    public void StartGameplay()
    {
        // But now, it just starts the delayed routine
        StartCoroutine(StartGameplayRoutine());
    }

    // This is the new coroutine that contains the delay
    private IEnumerator StartGameplayRoutine()
    {
        // 1. Wait for the specified amount of time
        yield return new WaitForSeconds(gameplayStartDelay);

        // 2. After waiting, execute the rest of the logic
        initialBackground.SetActive(false);
        mainGameplayBackground.SetActive(true);

        platformSpawner.ActivateSpawner();
        platformSpawner.enabled = true;

        backgroundFollower.enabled = true;
    }
}