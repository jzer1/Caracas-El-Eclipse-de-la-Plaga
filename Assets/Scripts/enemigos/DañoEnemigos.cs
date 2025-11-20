using UnityEngine;
using UnityEngine.Rendering;

public class DañoEnemigos : MonoBehaviour
{
    public EnemyStats enemyStats;

    float CurrentMoveSpeed;
    float CurrentHealth;
    float CurrrentDamage;

    private Rigidbody2D rb;
    public float knockbackForce = 3f;
    public float knockbackDuration = 0.15f;

    public float despawnDistance = 20f; // Distancia para despawn
    Transform player;

    private bool isKnockback = false;

    // 🔹 Contador global de enemigos vivos
    public static int enemiesAlive = 0;

    void OnEnable()
    {
        enemiesAlive++;
        //Debug.Log("[DañoEnemigos] Enemigo activado. Enemigos vivos: " + enemiesAlive);
    }

    void Awake()
    {
        CurrentMoveSpeed = enemyStats.moveSpeed;
        CurrentHealth = enemyStats.maxHealth;
        CurrrentDamage = enemyStats.damage;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Despawn si está muy lejos del jugador
        if (Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }

    public void TakeDamage(float damage, Vector2 hitDirection)
    {
        CurrentHealth -= damage;

        if (rb != null)
        {
            StopAllCoroutines();
            StartCoroutine(ApplyKnockback(hitDirection));
        }

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator ApplyKnockback(Vector2 direction)
    {
        isKnockback = true;

        rb.linearVelocity = direction.normalized * knockbackForce;
        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector2.zero;

        isKnockback = false;
    }

    void Die()
    {
        Debug.Log("[DañoEnemigos] Enemigo ha muerto: " + gameObject.name);

        DropItems dropSystem = GetComponent<DropItems>();
        if (dropSystem != null)
        {
            dropSystem.DropItem();
        }

        // Aquí puedes agregar efectos de muerte, animaciones, etc.
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerNivel player = col.GetComponent<PlayerNivel>();
            if (player != null)
            {
                Vector2 enemyPosition = transform.position;
                // Le pasamos la posición del enemigo como origen del golpe
                player.TakeDamage(CurrrentDamage, enemyPosition);
            }
        }
    }

    private void OnDestroy()
    {
        enemiesAlive--;
        if (enemiesAlive < 0) enemiesAlive = 0;

        Debug.Log("[DañoEnemigos] Enemigo destruido. Enemigos vivos: " + enemiesAlive);

        // Avisar al spawner si existe
        EnemySpawner es = Object.FindFirstObjectByType<EnemySpawner>();
        if (es != null)
        {
            es.OnEnemyKilled();
        }

        // ✅ Si ya no queda ninguno, avisar al jugador para cambiar de nivel
        if (enemiesAlive == 0)
        {
            PlayerNivel playerNivel = Object.FindFirstObjectByType<PlayerNivel>();
            if (playerNivel != null)
            {
                Debug.Log("[DañoEnemigos] Último enemigo muerto. Avisando al PlayerNivel para cambiar de nivel.");
                playerNivel.OnAllEnemiesKilled();
            }
            else
            {
                Debug.LogWarning("[DañoEnemigos] No se encontró PlayerNivel en la escena.");
            }
        }
    }

    void ReturnEnemy()
    {
        EnemySpawner es = Object.FindFirstObjectByType<EnemySpawner>();
        transform.position = player.position + es.spawnPoints[Random.Range(0, es.spawnPoints.Count)].position;
    }
}
