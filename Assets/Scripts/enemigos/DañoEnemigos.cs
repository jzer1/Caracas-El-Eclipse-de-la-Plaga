using UnityEngine;
using UnityEngine.Rendering;

public class DañoEnemigos : MonoBehaviour

{
    public EnemyStats enemyStats;

    float CurrentMoveSpeed ;
    float CurrentHealth;
    float CurrrentDamage;

    void Awake()
    {
        CurrentMoveSpeed = enemyStats.moveSpeed;
        CurrentHealth = enemyStats.maxHealth;
        CurrrentDamage = enemyStats.damage;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerNivel player = col.gameObject.GetComponent<PlayerNivel>();
            player.TakeDamage(CurrrentDamage);
        }
        
    }

}
