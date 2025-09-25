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
        // Aquí puedes agregar efectos de muerte, animaciones, etc.
        Destroy(gameObject);
    }

}
