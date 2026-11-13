
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{
    public static ManagerScene Instance;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string gameOverScene = "GameOver";
    public string victoryScene = "Victory";

    [Header("Level Scenes")]
    public string[] levelScenes =
    {
        "Level1",
        "Level2",
        "Level3",
        "Level4"
    };

    private int currentLevel = 0;

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

    // Cargar una escena por nombre
    public void SceneChanger(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Volver al menu principal
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    // Empezar una partida nueva
    public void StartGame()
    {
        currentLevel = 0;
        LoadCurrentLevel();
    }

    // Cargar el nivel actual
    private void LoadCurrentLevel()
    {
        if (currentLevel >= 0 &&
            currentLevel < levelScenes.Length)
        {
            SceneManager.LoadScene(levelScenes[currentLevel]);
        }
        else
        {
            Debug.LogError("El nivel indicado no existe.");
        }
    }

    // Completar un nivel
    public void CompleteLevel()
    {
        if (currentLevel < levelScenes.Length - 1)
        {
            currentLevel++;
            LoadCurrentLevel();
        }
        else
        {
            SceneManager.LoadScene(victoryScene);
        }
    }

    // Cuando el jugador muere o falla la captura
    public void PlayerDied()
    {
        SceneManager.LoadScene(gameOverScene);
    }

    // Reiniciar el ultimo nivel desbloqueado
    public void Restart()
    {
        LoadCurrentLevel();
    }

    // Volver al menu desde Game Over
    public void ExitToMenu()
    {
        LoadMainMenu();
    }

    // Salir del juego
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
