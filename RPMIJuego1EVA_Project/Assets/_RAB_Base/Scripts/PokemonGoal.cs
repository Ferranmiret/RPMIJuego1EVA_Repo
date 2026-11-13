using UnityEngine;

public class PokemonGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Si el que choca con el Pokémon es el jugador
        if (other.CompareTag("Player"))
        {
            // Buscamos el script PlayerInteractor del jugador y disparamos la captura
            PlayerInteractor player = other.GetComponent<PlayerInteractor>();
            if (player != null)
            {
                player.TryCatchPokemon();
            }
        }
    }
}