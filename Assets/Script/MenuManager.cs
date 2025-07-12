using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene To Load")]
    public string gameSceneName = "GameScene";

    [Header("Character Selection")]
    public List<CharacterData> characterList;
    public Transform characterDisplayPoint; // An empty GameObject where the character model will spawn

    [Header("UI References")]
    public TextMeshProUGUI totalCoinsText;
    public Button playUnlockButton;
    public TextMeshProUGUI playUnlockButtonText;

    private int selectedCharacterIndex = 0;
    private GameObject currentCharacterInstance;

    void Start()
    {
        // Set initial UI state
        UpdateCharacterSelection();
        UpdateCoinDisplay();
    }

    private void UpdateCoinDisplay()
    {
        if (CurrencySystem.instance != null)
        {
            totalCoinsText.text = CurrencySystem.instance.TotalCoins.ToString();
        }
    }

    private void UpdateCharacterSelection()
    {
        // Get the currently selected character's data
        CharacterData currentCharacter = characterList[selectedCharacterIndex];

        // Display the character model
        if (currentCharacterInstance != null) Destroy(currentCharacterInstance);
        currentCharacterInstance = Instantiate(currentCharacter.characterDisplayPrefab, characterDisplayPoint);

        // Check if the character is unlocked
        bool isUnlocked = PlayerPrefs.GetInt(currentCharacter.characterID, 0) == 1 || currentCharacter.isUnlockedByDefault;

        if (isUnlocked)
        {
            playUnlockButtonText.text = "PLAY";
            playUnlockButton.onClick.RemoveAllListeners();
            playUnlockButton.onClick.AddListener(PlayGame);
        }
        else
        {
            playUnlockButtonText.text = $"UNLOCK ({currentCharacter.unlockPrice})";
            playUnlockButton.onClick.RemoveAllListeners();
            playUnlockButton.onClick.AddListener(UnlockCharacter);
        }
    }

    public void NextCharacter()
    {
        selectedCharacterIndex = (selectedCharacterIndex + 1) % characterList.Count;
        UpdateCharacterSelection();
    }

    public void PreviousCharacter()
    {
        selectedCharacterIndex--;
        if (selectedCharacterIndex < 0)
        {
            selectedCharacterIndex = characterList.Count - 1;
        }
        UpdateCharacterSelection();
    }

    private void UnlockCharacter()
    {
        CharacterData currentCharacter = characterList[selectedCharacterIndex];
        if (CurrencySystem.instance.SpendCoins(currentCharacter.unlockPrice))
        {
            // Purchase successful
            PlayerPrefs.SetInt(currentCharacter.characterID, 1); // Save unlocked state
            PlayerPrefs.Save();
            UpdateCharacterSelection(); // Refresh the UI
            UpdateCoinDisplay(); // Refresh coin display
        }
        else
        {
            // Not enough coins
            Debug.Log("Not enough coins to unlock " + currentCharacter.characterName);
        }
    }

    private void PlayGame()
    {
        // Save which character was selected so the game scene can load it
        PlayerPrefs.SetString("SelectedCharacterID", characterList[selectedCharacterIndex].characterID);
        SceneManager.LoadScene(gameSceneName);
    }
}