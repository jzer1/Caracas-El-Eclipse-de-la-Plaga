using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy/EnemyStats")]   
public class EnemyStats : ScriptableObject
{
    public float moveSpeed ;
    public int maxHealth;
    public int damage;
}
