using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerNivel : MonoBehaviour
{
    public PlayerStats playerStats;

    float currentHealth;
    float currentRecoveryRate;
    float currentMoveSpeed;
    float currentAttackPower;
    float currentDefense;

    [Header("Fragmentos / Level")]
    public int fragments;        // Los fragmentos actuales del jugador
    public int level;            // Nivel actual del héroe
    public int fragmentsCost;    // Cuántos fragmentos cuesta subir de nivel

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


    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            float effectiveDamage = Mathf.Max(damage - currentDefense, 0);
            currentHealth -= effectiveDamage;
            currentHealth = Mathf.Clamp(currentHealth, 0, playerStats.maxHealth);
            
            isInvincible = true;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }
    void Die()
    {
        Debug.Log("Player has died.");
    }
}
