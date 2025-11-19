using UnityEngine;
using UnityEngine.SceneManagement;
public class Partida : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void nuevaPartida()
    {
        Debug.Log("Nueva Partida Iniciada");
        SceneManager.LoadScene("mapa1");
        // Aquí puedes agregar la lógica para iniciar una nueva partida
    }
}
