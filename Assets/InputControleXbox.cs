using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private Vector2 _moveInput;
    private bool _jumpPressed;
    private bool _interactPressed;
    private bool _pausePressed;

    public Vector2 MoveInput => _moveInput;
    public bool JumpPressed => _jumpPressed;
    public bool InteractPressed => _interactPressed;
    public bool PausePressed => _pausePressed;

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        Debug.Log($"[INPUT] Move: {_moveInput}");
    }

    public void OnJump(InputValue value)
    {
        _jumpPressed = value.isPressed;
        Debug.Log($"[INPUT] Jump: {_jumpPressed}");
    }

    public void OnInteract(InputValue value)
    {
        _interactPressed = value.isPressed;
        Debug.Log($"[INPUT] Interact: {_interactPressed}");
    }

    public void OnPause(InputValue value)
    {
        _pausePressed = value.isPressed;
        Debug.Log($"[INPUT] Pause: {_pausePressed}");
    }
}
