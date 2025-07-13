using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Setup")]
    public List<CharacterData> characterList;
    public CharacterData defaultCharacter;
    public Transform spawnPoint;
    public CameraFollow mainCameraFollow;
    public PlatformSpawner platformSpawner; 

    void Awake()
    {
        string selectedCharacterID = PlayerPrefs.GetString("SelectedCharacterID", "default");
        CharacterData characterToSpawn = characterList.Find(character => character.characterID == selectedCharacterID);
        GameObject playerInstance;

        if (characterToSpawn != null)
        {
            playerInstance = Instantiate(characterToSpawn.characterGamePrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Selected character not found. Spawning default character.");
            playerInstance = Instantiate(defaultCharacter.characterGamePrefab, spawnPoint.position, Quaternion.identity);
        }

        if (mainCameraFollow != null && playerInstance != null)
        {
            mainCameraFollow.target = playerInstance.transform;
        }

        if (platformSpawner != null && playerInstance != null)
        {
            platformSpawner.playerTransform = playerInstance.transform;
        }

        if (UI_InputBridge.instance != null && playerInstance != null)
        {
            UI_InputBridge.instance.ConnectPlayer(playerInstance.GetComponent<MobileInputHandler>());
        }
    }
}