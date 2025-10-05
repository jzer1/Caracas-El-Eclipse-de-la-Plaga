using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float maxHealth = 5;
    public float moveSpeed = 5f;
    public float attackPower = 10;
    public float defense = 5;
    public float recoveryRate = 1; // health points recovered per second

    [Header("Extras")]
    public float critChance = 0.1f; // 10% crítico
    public float critMultiplier = 2f; // crítico hace el doble de daño
}
