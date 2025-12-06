using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Centralized hub for player input using Unity's new Input System.
/// This script reads input from an InputSystem_Actions asset and provides
/// clean data for other player-related scripts to use.
/// </summary>
public class Player_Input : MonoBehaviour
{
    // Public properties to expose input data to other scripts
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Vector2 LookScreenPosition { get; private set; }
    public bool AttackInput { get; private set; }

    private InputSystem_Actions _actions;

    private void Awake()
    {
        _actions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _actions.Player.Enable();
    }

    private void OnDisable()
    {
        _actions.Player.Disable();
    }

    private void Update()
    {
        // Continuously read the values from the input actions.
        MoveInput = _actions.Player.Move.ReadValue<Vector2>();
        LookInput = _actions.Player.Look.ReadValue<Vector2>();

        // For aiming with the mouse, we need the absolute screen position.
        // We get this directly from the mouse device in the new Input System.
        if (Mouse.current != null)
        {
            LookScreenPosition = Mouse.current.position.ReadValue();
        }
        AttackInput = _actions.Player.Attack.IsPressed();
    }
}