using UnityEngine;

public class UI_InputBridge : MonoBehaviour
{
    public static UI_InputBridge instance;

    private MobileInputHandler playerInputHandler;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void ConnectPlayer(MobileInputHandler handler)
    {
        playerInputHandler = handler;
    }

    public void OnLeftButtonDown()
    {
        if (playerInputHandler != null) playerInputHandler.MoveLeftDown();
    }

    public void OnRightButtonDown()
    {
        if (playerInputHandler != null) playerInputHandler.MoveRightDown();
    }

    public void OnPointerUp()
    {
        if (playerInputHandler != null) playerInputHandler.StopMove();
    }
}