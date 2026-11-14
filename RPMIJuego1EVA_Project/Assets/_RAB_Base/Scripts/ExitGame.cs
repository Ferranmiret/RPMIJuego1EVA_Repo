using UnityEngine;

public class ExitGame : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Sistema de salida iniciado");
    }

    public void QuitGame()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}