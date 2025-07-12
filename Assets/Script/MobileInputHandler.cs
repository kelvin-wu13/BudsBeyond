using UnityEngine;

public class MobileInputHandler : MonoBehaviour
{
    [HideInInspector] public int moveDirection = 0;

    public void MoveLeftDown() => moveDirection = -1;
    public void MoveRightDown() => moveDirection = 1;

    public void StopMove() => moveDirection = 0;
}
