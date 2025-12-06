using UnityEngine;

/// <summary>
/// Handles the player's movement in a top-down 2D environment using WASD keys.
/// Requires a Rigidbody2D component on the same GameObject.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Player_Input))]
public class Player_Movement : MonoBehaviour
{
    [Tooltip("The speed at which the player moves.")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Player_Input playerInput;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<Player_Input>();
    }

    void Update()
    {
        // Get movement input from our central input script.
        // The new Input System already provides a normalized Vector2 for gamepad sticks,
        // but WASD input is not normalized by default, so we still do it here for consistency.
        movement = playerInput.MoveInput;

        // Normalize the movement vector to prevent faster diagonal movement.
        movement.Normalize();
    }

    void FixedUpdate()
    {
        // Physics calculations should be in FixedUpdate.
        // Move the player's position based on input, speed, and fixed delta time.
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}