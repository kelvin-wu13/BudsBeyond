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
    // A list of platforms that are above the player and have not yet been scored.
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

        // Use a while loop in case the player passes multiple platforms in a single frame.
        // It checks if the player is higher than the lowest platform in the unscored list.
        while (unscoredPlatforms.Count > 0 && playerTransform.position.y > unscoredPlatforms[0].position.y)
        {
            currentScore++;
            scoreText.text = currentScore.ToString();

            // Remove the platform we just passed so it is not counted again.
            unscoredPlatforms.RemoveAt(0);
        }
    }

    // Called by the PlatformSpawner to add a new platform to our list.
    public void RegisterPlatform(Transform platformTransform)
    {
        unscoredPlatforms.Add(platformTransform);

        // Sort the list by Y position to ensure we are always checking against the lowest platform first.
        unscoredPlatforms = unscoredPlatforms.OrderBy(p => p.position.y).ToList();
    }
}