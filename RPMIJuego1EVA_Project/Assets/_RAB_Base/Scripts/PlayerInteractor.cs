using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; // Librería de carga/descarga de escenas

public class PlayerInteractor : MonoBehaviour
{
    [Header("Respawn Configuration")]
    public Transform respawnPoint; // Ref al punto de respawn para usar su posición
    public float respawnFallLimit = -10; // Valor negativo en eje Y a partir del cual se respawnea

    [Header("Point & Pokemon System")]
    public int points; // Cantidad de monedas actuales del player
    public int winPoints = 3; // Monedas totales del nivel (3 para Nivel 1, 5 para Nivel 2, 7 para Nivel 3, 10 para Nivel 4)
    public int sceneToLoad = 2; // Número de escena a cargar cuando se captura y se pasa de nivel

    [Range(0f, 100f)]
    public float maxCatchProbability = 100f; // Probabilidad máxima si se recogen el 100% de las monedas
    private bool hasTriedToCatch = false; // Evita que la comprobación se ejecute mil veces por segundo

    [Header("Reference Variables")]
    public Rigidbody playerRb; // Ref al rigidbody para resetear su motor de aceleración al respawnear

    [Header("SFX Configuration")]
    public AudioClip[] sfxList; // Array de sonidos
    public AudioSource sfxSource; // Ref al altavoz del player

    void Start()
    {
        points = 0;
        hasTriedToCatch = false;
    }

    void Update()
    {
        // Respawn por caída
        if (transform.position.y <= respawnFallLimit)
        {
            Respawn();
        }

        // Condición de victoria: Cuando cogemos todas las monedas, intentamos atrapar al Pokémon (solo una vez)
        if (points >= winPoints && !hasTriedToCatch)
        {
            hasTriedToCatch = true;
            TryCatchPokemon();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            points += 1; // Suma una moneda al total
            sfxSource.PlayOneShot(sfxList[2]); // Sonido de pick up
            other.gameObject.SetActive(false); // Apaga la moneda cogida
        }
        if (other.gameObject.CompareTag("Switch"))
        {
            sfxSource.PlayOneShot(sfxList[3]); // Sonido del switch
        }
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        sfxSource.PlayOneShot(sfxList[1]);

        // Resetea los dashes si usas el script de dash que hicimos antes
        PlayerDash dashScript = GetComponent<PlayerDash>();
        if (dashScript != null)
        {
            dashScript.ResetDashes();
        }
    }

    void TryCatchPokemon()
    {
        // CALCULADORA DE PROBABILIDAD:
        // Si coges todas las monedas (ej: 3 de 3), la probabilidad es del 100% (o el valor de maxCatchProbability).
        // Si coges menos, la probabilidad baja de forma proporcional.
        float catchProbability = ((float)points / winPoints) * maxCatchProbability;

        float roll = Random.Range(0f, 100f);

        Debug.Log("¡Intento de captura! Probabilidad: " + catchProbability + "% | Dado: " + roll);

        if (roll <= catchProbability)
        {
            Debug.Log("¡Enhorabuena! ¡Pokémon atrapado con éxito!");
            // Puedes añadir aquí un sonido de victoria si queréis
            Invoke("LoadNextScene", 1.5f); // Espera 1.5 segundos y pasa de nivel
        }
        else
        {
            Debug.Log("¡Vaya! El Pokémon se ha escapado... Reiniciando nivel para reintentar.");
            // Al fallar, recargamos la escena actual: las monedas vuelven a aparecer y el Pokémon sigue ahí
            Invoke("RestartCurrentScene", 1.5f);
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void RestartCurrentScene()
    {
        // Recarga exactamente el nivel en el que estás
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}