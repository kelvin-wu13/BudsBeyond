// Import the TextMeshPro library to work with UI Text
using System.Drawing;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    // Drag your UI Text element here in the Inspector
    public TextMeshProUGUI pointText;

    private int currentPoints = 0;
    private int maxPoints;

    void Awake()
    {
        // Set up the singleton instance
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
        maxPoints = FindObjectsOfType<Points>().Length;


        // Initialize the UI text at the start of the game
        UpdatePointText();
    }

    public void AddPoint()
    {
        currentPoints++;
        UpdatePointText();

        if (currentPoints >= maxPoints)
        {
            Debug.Log("You collected all the points!");
        }
    }

    // A dedicated method to update the text display
    private void UpdatePointText()
    {
        pointText.text = $"{currentPoints} / {maxPoints}";
    }
}