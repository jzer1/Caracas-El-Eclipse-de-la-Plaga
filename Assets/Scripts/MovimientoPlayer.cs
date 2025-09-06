using UnityEngine;

public class MovimientoPlayer : MonoBehaviour
{
    public float speed = 5f; // Velocidad del jugador
    Rigidbody2D rb;

    [HideInInspector] public float lastHorizontal;
    [HideInInspector] public float lastVertical;
    [HideInInspector] public Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        InputManagement();
    }

    void FixedUpdate()
    {
        Move();
    }

    void InputManagement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        if (movement.x != 0 || movement.y != 0)
        {
            lastHorizontal = movement.x;
            lastVertical = movement.y;
        }
    }

    void Move()
    {
        // Si no hay entrada de movimiento, la velocidad se establece en cero
        if (movement == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = movement * speed;
        }
    }


    public Vector2Int MovementDir
    {
        get
        {
            return new Vector2Int(
                Mathf.RoundToInt(Mathf.Sign(movement.x)),
                Mathf.RoundToInt(Mathf.Sign(movement.y))
            );
        }
    }
}
