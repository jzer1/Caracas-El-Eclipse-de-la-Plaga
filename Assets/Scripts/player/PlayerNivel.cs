using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class PlayerNivel : MonoBehaviour
{

    [Header("Retroceso (Knockback)")]
    public float knockbackForce = 10f;    // Fuerza del empuje (ajusta según necesites)
    public float knockbackDuration = 0.25f; // Duración del retroceso en segundos

    // El estado CRUCIAL que será leído por el script de movimiento
    public bool isKnockedBack = false;

    private Rigidbody2D rb;

    public PlayerStats playerStats;
    private Animator Animator;
    public PlayerHealthUI healthUI;


    float currentHealth;
    float currentRecoveryRate;
    float currentMoveSpeed;
    float currentAttackPower;
    float currentDefense;

    [Header("Fragmentos / Level")]
    public int fragments;        // Los fragmentos actuales del jugador
    public int level;            // Nivel actual del héroe
    public int fragmentsCost;    // Cuántos fragmentos cuesta subir de nivel
    public FragmentUI fragmentUI;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int fragmentCostIncrease; // Ahora aumenta el costo en fragmentos
    }

    [Header("I-Frame")]
    public float invincibilityDuration = 1f; // Duración de los I-Frames en segundos
    private float invincibilityTimer = 0f;
    private bool isInvincible = false;

    public List<LevelRange> levelRanges;

    private void Awake()
    {
        currentHealth = playerStats.maxHealth;
        currentRecoveryRate = playerStats.recoveryRate;
        currentMoveSpeed = playerStats.moveSpeed;
        currentAttackPower = playerStats.attackPower;
        currentDefense = playerStats.defense;
        rb = GetComponent<Rigidbody2D>();

        // **VERIFICA ESTO:** Si sale 'null', el Rigidbody2D no está adjunto.
        if (rb == null)
        {
            Debug.LogError("PlayerNivel: ¡Rigidbody2D NO ENCONTRADO! El retroceso fallará.");
        }

        Animator = GetComponent<Animator>();
        if (Animator == null)
        {
            Debug.LogError("PlayerNivel: ¡Animator NO ENCONTRADO!");
        }
    }

    private void Start()
    {
        fragmentsCost = levelRanges[0].fragmentCostIncrease; // El costo inicial
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer >= invincibilityDuration)
            {
                isInvincible = false;
                invincibilityTimer = 0f;
            }
        }
    }

    // Método para añadir fragmentos (cuando matas enemigos)
    public void AddFragments(int amount)
    {
        fragments += amount;
        fragmentUI.UpdateCount(fragments);

    }

    // Método que intenta subir de nivel si el jugador tiene fragmentos suficientes
    public bool TryLevelUp()
    {
        if (fragments >= fragmentsCost)
        {
            fragments -= fragmentsCost;
            level++;

            int costIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    costIncrease = range.fragmentCostIncrease;
                    break;
                }
            }

            fragmentsCost += costIncrease;

            // Aquí puedes agregar mejoras al subir de nivel
            UpgradeStats();

            return true;
        }

        return false; // No tenía suficientes fragmentos
    }

    // Ejemplo de mejora de stats
    void UpgradeStats()
    {
        playerStats.maxHealth += 10;
        playerStats.attackPower += 2;
        currentHealth = playerStats.maxHealth;
    }


    public void TakeDamage(float damage, Vector2 sourcePosition)
    {
        if (!isInvincible)
        {
            float effectiveDamage = Mathf.Max(damage - currentDefense, 0);
            currentHealth -= effectiveDamage;
            currentHealth = Mathf.Clamp(currentHealth, 0, playerStats.maxHealth);


            healthUI.SetHealth(currentHealth, playerStats.maxHealth);
            Animator.SetTrigger("Hurt");

            ApplyKnockback(sourcePosition);

            isInvincible = true;
            if (currentHealth <= 0)
            {
                
                if(GameManager.instance != null)
                {
                    GameManager.instance.GameOver();

                }
                Die();

                
            }
        }
    }

    private void ApplyKnockback(Vector2 sourcePosition)
    {
        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero; // Detiene cualquier movimiento previo
        Vector2 knockbackDirection = ((Vector2)transform.position - sourcePosition).normalized;

        Debug.Log("APLICANDO KNOCKBACK. Dirección: " + knockbackDirection + " Fuerza: " + knockbackForce);
 
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(StopKnockbackRoutine(knockbackDuration));
    }

    IEnumerator StopKnockbackRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        isKnockedBack = false;
        rb.linearVelocity = Vector2.zero; // Detiene el movimiento después del knockback
    }

    void Die()
    {
        Debug.Log("Player has died.");
    }

    public void Curar(float cantidad)
    {
        currentHealth = Mathf.Min(currentHealth + cantidad, playerStats.maxHealth);
        Debug.Log("Vida actual: " + currentHealth);

        if (healthUI != null)
        {
            healthUI.SetHealth(currentHealth, playerStats.maxHealth);
        }
    }
    // Llamado cuando muere un enemigo para ver si ya no queda ninguno
    public void CheckEnemiesRemaining()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            NextLevel();
        }
    }

    public void OnAllEnemiesKilled()
    {
        Debug.Log("[PlayerNivel] Todos los enemigos han sido derrotados. Cambiando de nivel...");
        NextLevel();
    }

    void NextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        Debug.Log("[PlayerNivel] Escena actual: " + currentIndex +
                " | Siguiente: " + nextIndex +
                " | Total escenas en BuildSettings: " + SceneManager.sceneCountInBuildSettings);

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("[PlayerNivel] No hay más escenas en BuildSettings. Fin del juego o falta configuración.");
            // Aquí podrías mostrar pantalla de victoria final.
        }
    }



}
