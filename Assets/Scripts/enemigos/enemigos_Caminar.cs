using UnityEngine;

public class enemigos_Caminar : MonoBehaviour
{
    Transform player;

    public float moveSpeed;
    Vector3 initialScale;
    void Start()
    {
        player = Object.FindFirstObjectByType<MovimientoPlayer>().transform;
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Movimiento hacia el jugador
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Volteo manteniendo el tamaño original
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
    }
}
