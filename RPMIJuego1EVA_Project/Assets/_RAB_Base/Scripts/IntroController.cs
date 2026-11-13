

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [SerializeField] private float introDuration = 5f;
    [SerializeField] private string mainMenuScene = "SCN_MainMenu";

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(introDuration);

        SceneManager.LoadScene(mainMenuScene);
    }
}
