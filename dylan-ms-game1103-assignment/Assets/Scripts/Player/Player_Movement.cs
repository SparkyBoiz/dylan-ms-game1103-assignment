using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Player_Input))]
public class Player_Movement : MonoBehaviour
{
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
        movement = playerInput.MoveInput;

        movement.Normalize();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}