using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterData : ScriptableObject
{
    public string characterID;
    public string characterName;
    public GameObject characterDisplayPrefab;
    public GameObject characterGamePrefab;
    public int unlockPrice;
    public bool isUnlockedByDefault = false;
        
    [Header("Character Stats")]
    public float jumpSpeed = 12f;

    [Header("Power-up")]
    public GameObject powerUpPrefab;
}