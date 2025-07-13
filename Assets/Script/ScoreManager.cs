using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Tracking")]
    public Transform playerTransform;

    [Header("UI")]
    public TextMeshProUGUI scoreText;

    private int currentScore = 0;
    private List<Transform> unscoredPlatforms = new List<Transform>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        scoreText.text = "0";
    }

    void Update()
    {
        if (playerTransform == null) return;

        while (unscoredPlatforms.Count > 0 && unscoredPlatforms[0] == null)
        {
            unscoredPlatforms.RemoveAt(0);
        }

        while (unscoredPlatforms.Count > 0 && playerTransform.position.y > unscoredPlatforms[0].position.y)
        {
            currentScore++;
            scoreText.text = currentScore.ToString();

            unscoredPlatforms.RemoveAt(0);
        }
    }

    public void RegisterPlatform(Transform platformTransform)
    {
        unscoredPlatforms.Add(platformTransform);
    }
}