using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShrink : MonoBehaviour
{
    [Header("Shrink Settings")]
    public KeyCode shrinkKey = KeyCode.Q;
    public float smallScale = 0.5f;

    private Vector3 originalScale;
    private bool isSmall = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current[Key.Q].wasPressedThisFrame)
        {
            ToggleShrink();
        }
    }

    void ToggleShrink()
    {
        if (isSmall)
        {
            transform.localScale = originalScale;
            isSmall = false;

            Debug.Log("¡Tamaño original recuperado!");
        }
        else
        {
            transform.localScale = originalScale * smallScale;
            isSmall = true;

            Debug.Log("¡Pelota encogida!");
        }
    }

    public void ResetSize()
    {
        transform.localScale = originalScale;
        isSmall = false;
    }
}