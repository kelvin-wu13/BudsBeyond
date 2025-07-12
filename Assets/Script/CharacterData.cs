using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterData : ScriptableObject
{
    [Header("Character Stats")]
    public float jumpSpeed = 12f;

    [Header("Power-up")]
    public GameObject powerUpPrefab;
}