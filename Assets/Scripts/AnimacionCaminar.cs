using UnityEngine;

public class AnimacionCaminar : MonoBehaviour
{
    Animator am;
    MovimientoPlayer mp;
    SpriteRenderer sr;

    void Start()
    {
        am = GetComponent<Animator>();
        mp = GetComponent<MovimientoPlayer>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 movement = mp.movement;

        if (movement != Vector2.zero)
        {
            am.SetBool("Move", true);
            SpriteDireccion();
        }
        else
        {
            am.SetBool("Move", false);
        }
    }

    void SpriteDireccion()
    {
        
        if (mp.lastHorizontal < 0)
        {
            sr.flipX = true;
        }
        else if (mp.lastHorizontal > 0)
        {
            sr.flipX = false;
        }

       
    }
}
