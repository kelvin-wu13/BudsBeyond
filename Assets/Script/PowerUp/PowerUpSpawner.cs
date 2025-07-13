using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                CharacterData characterData = playerController.GetCharacterData();
                if (characterData != null && characterData.powerUpPrefab != null)
                {
                    Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
                    Instantiate(characterData.powerUpPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}