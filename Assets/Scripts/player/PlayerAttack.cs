using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public WeaponStats weapon;          // referencia al arma (ScriptableObject)
    public GameObject swordHitbox;      // referencia al objeto hitbox
    private Animator animator;          // animador del player
    private float lastAttackTime = 0f;  // controla cooldown
    private bool isAttacking = false;   // evita repetir animación mientras ataca

    private SpriteRenderer sr;

    void Start()
    {
        animator = GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Solo ataca si presiono click, no está atacando y pasó el cooldown
        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time >= lastAttackTime + weapon.attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        isAttacking = true;

        // Dispara la animación (los Animation Events se encargarán del hitbox)
        animator.SetTrigger("Attack");
    }

    // 🔹 Método llamado por Animation Event en el frame donde comienza el golpe
    public void OnAttack()
    {
        // Ajusta el offset si tu arma lo necesita
        float offset = sr.flipX ? weapon.hitboxOffsetLeft : weapon.hitboxOffset;
        swordHitbox.transform.localPosition = new Vector3(offset, 0f, 0f);

        // Pasa el daño del arma al hitbox
        SwordHitbox hb = swordHitbox.GetComponent<SwordHitbox>();
        hb.damage = weapon.damage;

        // Activa el hitbox
        swordHitbox.SetActive(true);
    }

    // 🔹 Método llamado por Animation Event en el frame donde termina el golpe
    public void OnAttackEnd()
    {
        swordHitbox.SetActive(false);
        isAttacking = false; // permite volver a atacar
    }
}