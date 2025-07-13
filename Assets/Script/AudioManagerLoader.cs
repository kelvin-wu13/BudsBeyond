using UnityEngine;

public class AudioManagerLoader : MonoBehaviour
{
    public GameObject audioManagerPrefab;

    void Awake()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(audioManagerPrefab);
        }
    }
}