using UnityEngine;

public class PocionCuracion : MonoBehaviour, ICollectible
{
    [Header("Curación")]
    public float cantidadCuracion = 20f; // cantidad de vida que recupera

    public void Collect(PlayerNivel player)
    {
        player.Curar(cantidadCuracion);
        Debug.Log($"El jugador se curó {cantidadCuracion} puntos de vida.");
        Destroy(gameObject);
    }
}
