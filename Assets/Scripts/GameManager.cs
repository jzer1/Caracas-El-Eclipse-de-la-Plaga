using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public Button reiniciarButton;
    public Button menuButton;

    private bool gameOverActivo = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);  <- lo quitamos
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        if (reiniciarButton != null)
            reiniciarButton.onClick.AddListener(ReiniciarJuego);

        if (menuButton != null)
            menuButton.onClick.AddListener(VolverAlMenu);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (gameOverActivo)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReiniciarJuego();
            }

            if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
            {
                VolverAlMenu();
            }
        }
    }


    public void GameOver()
    {
        if (gameOverActivo) return;
        gameOverActivo = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);


        if (finalScoreText != null)
            finalScoreText.text = "Game Over";

        Time.timeScale = 0f; // Pausar el juego
    }

    public void ReiniciarJuego()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        gameOverActivo = false;
        Time.timeScale = 1f; // Reanudar tiempo
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }


    public void VolverAlMenu()
    {
        Time.timeScale = 1f; // Reanudar el tiempo
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");

        gameOverPanel.SetActive(false);
    }

}
