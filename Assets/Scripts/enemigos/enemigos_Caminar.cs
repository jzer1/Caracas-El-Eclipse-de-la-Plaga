using UnityEngine;

public class enemigos_Caminar : MonoBehaviour
{
    // Referencias
    [SerializeField] private Transform player;
    public EnemyStats enemyStats;
    private Vector3 initialScale;

    // IA y Evasión
    [Header("IA y Evasión")]
    public LayerMask obstacleLayer;
    public float detectionDistance = 1.0f;
    public float avoidanceTime = 0.5f;
    private float avoidanceTimer;
    private int avoidanceDirection = 1; // 1 (arriba) o -1 (abajo)

    void Start()
    {
        // Obtención de la referencia del jugador
        MovimientoPlayer playerComponent = Object.FindFirstObjectByType<MovimientoPlayer>();

        if (playerComponent != null)
        {
            player = playerComponent.transform;
        }
        else
        {
            Debug.LogError("PlayerNivel no encontrado. Deshabilitando persecución.");
            enabled = false;
            return;
        }

        // Almacena la escala inicial (debe ser X positiva)
        initialScale = transform.localScale;

        // **CORRECCIÓN DE ORIENTACIÓN:** Asegura el volteo correcto al spawnear
        UpdateFlip();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 targetPosition = player.position;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // ==========================================================
        // LÓGICA DE EVASIÓN DE OBSTÁCULOS
        // ==========================================================
        if (avoidanceTimer > 0)
        {
            // MODO EVASIÓN: Moverse lateralmente
            avoidanceTimer -= Time.deltaTime;
            // Vector perpendicular para moverse hacia 'arriba' o 'abajo'
            Vector3 perpendicularDirection = new Vector3(-moveDirection.y, moveDirection.x, 0);
            transform.position += perpendicularDirection * avoidanceDirection * enemyStats.moveSpeed * Time.deltaTime;
        }
        else
        {
            // MODO PERSECUCIÓN Y DETECCIÓN
            // Detección con Linecast: Verifica si hay un obstáculo en la dirección de movimiento
            RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + moveDirection * detectionDistance, obstacleLayer);

            if (hit.collider != null)
            {
                // Obstáculo detectado: Inicia el modo evasión
                avoidanceTimer = avoidanceTime;
                avoidanceDirection = (Random.value > 0.5f) ? 1 : -1;
            }
            else
            {
                // No hay obstáculos: Perseguir al jugador (movimiento lineal)
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemyStats.moveSpeed * Time.deltaTime);
            }
        }

        // ==========================================================
        // LÓGICA DE VOLTEO (Flip)
        // ==========================================================
        UpdateFlip();
    }

    // Función de volteo que usa solo la escala
    void UpdateFlip()
    {
        if (player.position.x < transform.position.x)
        {
            // Mira a la izquierda (Escala X negativa)
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
        else
        {
            // Mira a la derecha (Escala X positiva)
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
    }

    // Opcional: Visualiza la detección en el editor
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector3 moveDirection = (player.position - transform.position).normalized;
            Gizmos.color = (avoidanceTimer > 0) ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position, transform.position + moveDirection * detectionDistance);
        }
    }
}