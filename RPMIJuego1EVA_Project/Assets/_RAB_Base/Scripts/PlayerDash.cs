using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashForce = 40f;          // Fuerza del dash largo
    public int totalDashesLeft = 3;        // Límite total
    public KeyCode dashKey = KeyCode.F;    // Tecla de dash

    private Rigidbody rb;
    private PlayerController playerController;

    private bool isGrounded = true;
    private float timeSinceLeftGround = 0f;
    private float dashCooldown = 0.5f;     // Tiempo mínimo entre dash y dash para que no sean seguidos
    private float lastDashTime = -999f;    // Control del cooldown

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!isGrounded)
        {
            timeSinceLeftGround += Time.deltaTime;
        }

        if (Keyboard.current != null && Keyboard.current[Key.F].wasPressedThisFrame)
        {
            // Comprobamos: en el aire, con dashes, fuera de rampas y respetando el cooldown entre dashes
            if (!isGrounded && totalDashesLeft > 0 && timeSinceLeftGround > 0.15f && (Time.time - lastDashTime) >= dashCooldown)
            {
                PerformDash();
            }
        }
    }

    void PerformDash()
    {
        totalDashesLeft--;
        lastDashTime = Time.time; // Guardamos el momento exacto del dash

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

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(dashDir * dashForce, ForceMode.Impulse);

        Debug.Log("¡Dash ejecutado! Te quedan: " + totalDashesLeft);
    }

    // Método público para llamar cuando el jugador muera, caiga al vacío o reinicie el nivel
    public void ResetDashes()
    {
        totalDashesLeft = 3;
        Debug.Log("¡Dashes reseteados a 3!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            timeSinceLeftGround = 0f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            timeSinceLeftGround = 0f;
        }
    }
}