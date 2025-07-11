using UnityEngine;

public class MobileInputHandler : MonoBehaviour
{
    [HideInInspector] public int moveDirection = 0; // -1 = left, 1 = right, 0 = idle
    [HideInInspector] public bool jumpPressed = false;

    // Called on button press (PointerDown)
    public void MoveLeftDown() => moveDirection = -1;
    public void MoveRightDown() => moveDirection = 1;
    public void JumpButtonDown() => jumpPressed = true;

    // Called on button release (PointerUp)
    public void StopMove() => moveDirection = 0;
    public void StopJump() => jumpPressed = false; // optional, depends on your jump logic
}
