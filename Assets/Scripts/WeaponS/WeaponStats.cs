using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public string weapon;
    public int damage;
    public float attackCooldown;
    public float attackRange;
    public float hitboxduration;
    public float hitboxOffset = 0.0f;
    public float hitboxOffsetLeft = -0.8f;
}
