using UnityEngine;
using UnityEngine.Rendering;

public class DañoEnemigos : MonoBehaviour

{
    public EnemyStats enemyStats;

    float CurrentMoveSpeed ;
    float CurrentHealth;
    float CurrrentDamage;

    private Rigidbody2D rb;
    public float knockbackForce = 3f;
    public float knockbackDuration = 0.15f;

    private bool isKnockback= false;

    void Awake()
    {
        CurrentMoveSpeed = enemyStats.moveSpeed;
        CurrentHealth = enemyStats.maxHealth;
        CurrrentDamage = enemyStats.damage;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage ,Vector2 hitDirection)
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
        if (col.CompareTag("Player") )
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
  
}
