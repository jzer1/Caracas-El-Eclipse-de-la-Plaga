using UnityEngine;

public class PlayerCollector : MonoBehaviour

{
    public int fragmentosTotales = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible collectible = collision.GetComponent<ICollectible>();

        if (collectible != null)
        {
            collectible.Collect(GetComponent<PlayerNivel>());
        }
    }

    public void AgregarFragmentos(int cantidad)
    {
        fragmentosTotales += cantidad;
        Debug.Log("Total de fragmentos: " + fragmentosTotales);
    }
}
