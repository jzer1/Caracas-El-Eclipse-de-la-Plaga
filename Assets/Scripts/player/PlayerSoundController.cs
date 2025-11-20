using UnityEngine;

public class PlayersoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoAtacar;
    public AudioClip sonidodaño;


    public void Playatacar()
    {
        audioSource.PlayOneShot(sonidoAtacar);
    }

    public void Playdaño()
    {
        audioSource.PlayOneShot(sonidodaño);
    }

}
