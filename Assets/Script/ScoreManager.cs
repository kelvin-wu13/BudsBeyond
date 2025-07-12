using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Tracking")]
    [Tooltip("Drag the player's GameObject here to track its position.")]
    public Transform playerTransform;

    [Header("UI")]
    [Tooltip("Drag the TextMeshPro UI element that will display the score.")]
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

        unscoredPlatforms = unscoredPlatforms.OrderBy(p => p.position.y).ToList();
    }
}