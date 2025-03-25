using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class TouchInput : MonoBehaviour
{
    // Eventos públicos
    public UnityEvent<Vector2> onTouchPosition;
    public UnityEvent onTouchPress;
    public UnityEvent onDoubleTap;
    public UnityEvent<Vector2> onSwipe;

    private InputAction _touchPosition;
    private InputAction _touchPress;
    private InputAction _doubleTap;
    private InputAction _swipe;

    void Awake()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();

        _touchPosition = playerInput.actions["TouchPosition"];
        _touchPress = playerInput.actions["TouchPress"];
        _doubleTap = playerInput.actions["DoubleTap"];
        _swipe = playerInput.actions["Swipe"];
    }

    void OnEnable()
    {
        _touchPosition.performed += HandlePosition;
        _touchPress.performed += HandlePress;
        _doubleTap.performed += HandleDoubleTap;
        _swipe.performed += HandleSwipe;
    }

    void OnDisable()
    {
        _touchPosition.performed -= HandlePosition;
        _touchPress.performed -= HandlePress;
        _doubleTap.performed -= HandleDoubleTap;
        _swipe.performed -= HandleSwipe;
    }

    private void HandlePosition(InputAction.CallbackContext context)
    {
        onTouchPosition?.Invoke(context.ReadValue<Vector2>());
    }

    private void HandlePress(InputAction.CallbackContext context)
    {
        onTouchPress?.Invoke();
    }

    private void HandleDoubleTap(InputAction.CallbackContext context)
    {
        onDoubleTap?.Invoke();
    }

    private void HandleSwipe(InputAction.CallbackContext context)
    {
        onSwipe?.Invoke(context.ReadValue<Vector2>());
    }
}