using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int maxHealth = 5;
    public float moveSpeed = 5f;
    public int attackPower = 10;
    public int defense = 5;
    public int recoveryRate = 1; // health points recovered per second

    [Header("Extras")]
    public float critChance = 0.1f; // 10% crítico
    public float critMultiplier = 2f; // crítico hace el doble de daño
}
