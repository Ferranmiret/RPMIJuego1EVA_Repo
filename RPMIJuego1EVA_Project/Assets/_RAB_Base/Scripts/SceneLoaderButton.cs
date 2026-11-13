using UnityEngine;

public class SceneLoaderButton : MonoBehaviour
{
    // Método para reiniciar el nivel actual a través del gestor
    public void CallRestart()
    {
        if (ManagerScene.Instance != null)
        {
            ManagerScene.Instance.Restart();
        }
        else
        {
            Debug.LogError("No se encuentra el ManagerScene en la escena.");
        }
    }

    // Método para volver al menú principal a través del gestor
    public void CallExitToMenu()
    {
        if (ManagerScene.Instance != null)
        {
            ManagerScene.Instance.ExitToMenu();
        }
        else
        {
            Debug.LogError("No se encuentra el ManagerScene en la escena.");
        }
    }
}