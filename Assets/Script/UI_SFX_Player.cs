using UnityEngine;

public class UI_SFX_Player : MonoBehaviour
{
    public void PlayButtonClick()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX("Button");
        }
    }
}