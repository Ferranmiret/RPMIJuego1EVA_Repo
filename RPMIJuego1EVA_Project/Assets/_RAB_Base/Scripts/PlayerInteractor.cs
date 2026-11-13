using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Respawn Configuration")]
    public Transform respawnPoint;
    public float respawnFallLimit = -10;

    [Header("Point & Pokemon System")]
    public int points;
    public int winPoints = 3; // Monedas totales necesarias para el 100%

    [Range(0f, 100f)]
    public float maxCatchProbability = 100f;

    [Header("Reference Variables")]
    public Rigidbody playerRb;

    [Header("SFX Configuration")]
    public AudioClip[] sfxList;
    public AudioSource sfxSource;

    void Start()
    {
        points = 0;
    }

    void Update()
    {
        // Respawn por caída al vacío
        if (transform.position.y <= respawnFallLimit)
        {
            Respawn();
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
            points += 1;
            sfxSource.PlayOneShot(sfxList[2]);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Switch"))
        {
            sfxSource.PlayOneShot(sfxList[3]);
        }
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        sfxSource.PlayOneShot(sfxList[1]);

        PlayerDash dashScript = GetComponent<PlayerDash>();
        if (dashScript != null)
        {
            dashScript.ResetDashes();
        }
    }

    public bool TryCatchPokemon()
    {
        float catchProbability = ((float)points / winPoints) * maxCatchProbability;

        if (catchProbability > 100f) catchProbability = 100f;

        float roll = Random.Range(0f, 100f);

        Debug.Log("--- INTENTO DE CAPTURA ---");
        Debug.Log("Monedas: " + points + "/" + winPoints);
        Debug.Log("Probabilidad real: " + catchProbability + "%");
        Debug.Log("Número obtenido por el dado: " + roll);

        if (roll <= catchProbability)
        {
            Debug.Log("¡Pokémon atrapado con éxito! Pasando de nivel...");
            // Llama a la progresión centralizada del proyecto
            Invoke("LoadNextLevel", 1.5f);
            return true; // Devuelve true para que el Pokémon desaparezca
        }
        else
        {
            Debug.Log("¡El Pokémon se ha escapado! Game Over en 2 segundos...");
            // Espera 2 segundos antes de ir a Game Over mediante el ManagerScene
            Invoke("TriggerGameOver", 2.0f);
            return false; // Devuelve false para que el Pokémon siga ahí
        }
    }

    void LoadNextLevel()
    {
        if (ManagerScene.Instance != null)
        {
            ManagerScene.Instance.CompleteLevel(); //[cite: 1]
        }
        else
        {
            Debug.LogError("ManagerScene no encontrado en la escena.");
        }
    }

    void TriggerGameOver()
    {
        if (ManagerScene.Instance != null)
        {
            ManagerScene.Instance.PlayerDied(); //[cite: 1]
        }
        else
        {
            Debug.LogError("ManagerScene no encontrado en la escena.");
        }
    }
}