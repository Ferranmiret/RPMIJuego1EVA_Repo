using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Respawn Configuration")]
    public Transform respawnPoint;
    public float respawnFallLimit = -10f;

    [Header("Point & Pokemon System")]
    public int points;
    public int winPoints = 3;

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
        if (sfxList.Length > 1 && sfxList[1] != null)
        {
            sfxSource.PlayOneShot(sfxList[1]);
        }

        enabled = false;
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.isKinematic = true;
        }

        Debug.Log("¡El jugador ha muerto! Game Over en 2 segundos...");
        Invoke("TriggerGameOver", 2.0f);
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
            Invoke("LoadNextLevel", 1.5f);
            return true;
        }
        else
        {
            Debug.Log("¡El Pokémon se ha escapado! Game Over en 2 segundos...");
            Invoke("TriggerGameOver", 2.0f);
            return false;
        }
    }

    void LoadNextLevel()
    {
        if (ManagerScene.Instance != null)
        {
            ManagerScene.Instance.CompleteLevel();
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
            ManagerScene.Instance.PlayerDied();
        }
        else
        {
            Debug.LogError("ManagerScene no encontrado en la escena.");
        }
    }
}