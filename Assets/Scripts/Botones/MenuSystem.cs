using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Jugar()
    {
        Debug.Log("Cargando la siguiente escena.");
        SceneManager.LoadScene("Partida");

    }

    public void Salir()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}
