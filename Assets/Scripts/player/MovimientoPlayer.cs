    using UnityEngine;

public class MovimientoPlayer : MonoBehaviour
{

    Rigidbody2D rb;

    private PlayerNivel playerNivel;

    [HideInInspector] public float lastHorizontal;
    [HideInInspector] public float lastVertical;
    [HideInInspector] public Vector2 movement;

    public PlayerStats PlayerStats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerNivel = GetComponent<PlayerNivel>();
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
        if (playerNivel != null && playerNivel.isKnockedBack)
        {
            return;
        }
        // Si no hay entrada de movimiento, la velocidad se establece en cero
        if (movement == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = movement * PlayerStats.moveSpeed;
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
