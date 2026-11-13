
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{
    public static ManagerScene Instance;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuScene = "SCN_MainMenu";
    [SerializeField] private string gameOverScene = "SCN_GameOver";
    [SerializeField] private string victoryScene = "SCN_Victory";

    [Header("Level Scenes")]
    [SerializeField]
    private string[] levelScenes =
    {
        "SCN_Jacobo",
        "SCN_JuanPablo",
        "SCN_Ferran",
        "SCN_Christian"
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

    // Empezar una partida nueva
    public void StartGame()
    {
        currentLevel = 0;
        LoadCurrentLevel();
    }

    // Cargar el nivel actual
    private void LoadCurrentLevel()
    {
        if (currentLevel >= 0 && currentLevel < levelScenes.Length)
        {
            SceneManager.LoadScene(levelScenes[currentLevel]);
        }
        else
        {
            Debug.LogError("No se encuentra el nivel: " + currentLevel);
        }
    }

    // Captura conseguida
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

    // Jugador muerto o captura fallida
    public void PlayerDied()
    {
        SceneManager.LoadScene(gameOverScene);
    }

    // Reiniciar el ultimo nivel desbloqueado
    public void Restart()
    {
        LoadCurrentLevel();
    }

    // Volver al menu principal
    public void ExitToMenu()
    {
        currentLevel = 0;
        SceneManager.LoadScene(mainMenuScene);
    }

    // Salir del juego
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}