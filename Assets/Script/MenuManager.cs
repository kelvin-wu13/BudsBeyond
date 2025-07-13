using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene To Load")]
    public string gameSceneName = "Tester";

    [Header("Character Selection")]
    public List<CharacterData> characterList;
    public Transform characterDisplayPoint;

    [Header("UI References")]
    public TextMeshProUGUI totalCoinsText;
    public Button playUnlockButton;
    public TextMeshProUGUI playUnlockButtonText;

    private int selectedCharacterIndex = 0;
    private GameObject currentCharacterInstance;

    void Start()
    {
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
        CharacterData currentCharacter = characterList[selectedCharacterIndex];

        if (currentCharacterInstance != null) Destroy(currentCharacterInstance);
        currentCharacterInstance = Instantiate(currentCharacter.characterDisplayPrefab, characterDisplayPoint);

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
            PlayerPrefs.SetInt(currentCharacter.characterID, 1);
            PlayerPrefs.Save();
            UpdateCharacterSelection();
            UpdateCoinDisplay();
        }
        else
        {
            Debug.Log("Not enough coins to unlock " + currentCharacter.characterName);
        }
    }

    private void PlayGame()
    {
        PlayerPrefs.SetString("SelectedCharacterID", characterList[selectedCharacterIndex].characterID);
        SceneManager.LoadScene(gameSceneName);
    }
}