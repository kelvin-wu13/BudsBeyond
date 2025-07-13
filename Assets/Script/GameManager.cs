using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject initialBackground;
    public List<GameObject> mainGameplayBackgrounds;
    public PlatformSpawner platformSpawner;
    public float gameplayStartDelay = 1.5f;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBGM("In-Game");
        }

        Time.timeScale = 1f;
        initialBackground.SetActive(true);
        platformSpawner.enabled = false;
        foreach (GameObject bg in mainGameplayBackgrounds) { bg.SetActive(false); }
    }

    public void StartGameplay()
    {
        StartCoroutine(StartGameplayRoutine());
    }

    private IEnumerator StartGameplayRoutine()
    {
        yield return new WaitForSeconds(gameplayStartDelay);

        initialBackground.SetActive(false);
        if (mainGameplayBackgrounds.Count > 0)
        {
            mainGameplayBackgrounds[Random.Range(0, mainGameplayBackgrounds.Count)].SetActive(true);
        }

        platformSpawner.ActivateSpawner();
        platformSpawner.enabled = true;
    }
}