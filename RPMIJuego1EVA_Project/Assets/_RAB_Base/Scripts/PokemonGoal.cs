using UnityEngine;

public class PokemonGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteractor player = other.GetComponent<PlayerInteractor>();
            if (player != null)
            {
                // Intentamos capturar y guardamos si hemos tenido éxito
                bool success = player.TryCatchPokemon();

                if (success)
                {
                    // El Pokémon desaparece al instante de ser capturado
                    gameObject.SetActive(false);
                }
            }
        }
    }
}