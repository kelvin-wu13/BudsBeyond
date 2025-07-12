using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("A list of all possible characters the player can choose.")]
    public List<CharacterData> characterList;

    [Tooltip("A default character to spawn if the selected one isn't found.")]
    public CharacterData defaultCharacter;

    [Tooltip("The position where the player will be spawned.")]
    public Transform spawnPoint;

    void Awake()
    {
        // Get the ID of the character selected in the main menu
        string selectedCharacterID = PlayerPrefs.GetString("SelectedCharacterID", "default");

        // Find the CharacterData that matches the selected ID
        CharacterData characterToSpawn = characterList.Find(character => character.characterID == selectedCharacterID);

        // If we found a matching character, spawn it.
        if (characterToSpawn != null)
        {
            Instantiate(characterToSpawn.characterGamePrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            // If no match was found (or it's the first time playing), spawn the default character.
            Debug.LogWarning("Selected character not found. Spawning default character.");
            Instantiate(defaultCharacter.characterGamePrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}