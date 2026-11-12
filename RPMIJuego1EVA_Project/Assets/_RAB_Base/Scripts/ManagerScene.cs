using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;

    private int currentLevel = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Cargar una escena determinada
    public void SceneChanger(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // Empezar el juego
    public void StartGame()
    {
        currentLevel = 1;
        SceneManager.LoadScene(1);
    }

    // Completar un nivel
    public void CompleteLevel()
    {
        if (currentLevel < 5)
        {
            currentLevel++;
            SceneManager.LoadScene(currentLevel);
        }
        else
        {
            SceneManager.LoadScene(6);
        }
    }

    // Cuando muere el jugador
    public void PlayerDied()
    {
        currentLevel = Mathf.Max(1, currentLevel - 1);
        SceneManager.LoadScene(currentLevel);
    }

    // Salir del juego
    public void ExitGame()
    {
        Debug.Log("El botón de salir del juego funciona.");
        Application.Quit();
    }
}


