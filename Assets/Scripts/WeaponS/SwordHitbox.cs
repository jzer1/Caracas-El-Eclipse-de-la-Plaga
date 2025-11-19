using UnityEngine;
using System.Collections.Generic;

public class SwordHitbox : MonoBehaviour
{
    [HideInInspector] public int damage = 1;
    public string enemyTag = "Enemy";

    private HashSet<int> hitIds = new HashSet<int>();

    void OnEnable()
    {
        hitIds.Clear(); // se limpia cada vez que el hitbox se activa
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // solo golpea a objetos con el tag correcto
        if (!other.CompareTag(enemyTag)) return;

        int id = other.gameObject.GetInstanceID();
        if (hitIds.Contains(id)) return; // ya le pegamos en esta activación

        hitIds.Add(id);

        // Buscar el componente DañoEnemigos en el objeto golpeado
        DañoEnemigos enemy = other.GetComponentInParent<DañoEnemigos>();
        if (enemy != null)
        {
            Vector2 hitDirection = (enemy.transform.position - transform.position).normalized;
            enemy.TakeDamage(damage, hitDirection);
        }
    }
}
