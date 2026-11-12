using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashForce = 15f;          
    private float lastSpacePressTime = 0f; 
    private float doublePressThreshold = 0.3f; 

    private Rigidbody rb;
    private PlayerController playerController; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float timeSinceLastPress = Time.time - lastSpacePressTime;

            
            if (timeSinceLastPress <= doublePressThreshold)
            {
                PerformDash();
            }

            lastSpacePressTime = Time.time;
        }
    }

    void PerformDash()
    {
        Vector2 moveInput = Vector2.zero;
        if (playerController != null)
        {
            moveInput = playerController.moveInput;
        }

        Vector3 dashDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        
        if (dashDir == Vector3.zero)
        {
            dashDir = transform.forward;
        }

        
        rb.AddForce(dashDir * dashForce, ForceMode.Impulse);
    }
}
