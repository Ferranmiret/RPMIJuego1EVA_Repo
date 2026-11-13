using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDoubleJump : MonoBehaviour
{
    [Header("Double Jump Settings")]
    public float jumpForce = 10f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool secondJumpUsed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            // Solo permite el segundo salto cuando estamos en el aire
            if (!isGrounded && !secondJumpUsed)
            {
                PerformSecondJump();
            }
        }
    }

    void PerformSecondJump()
    {
        secondJumpUsed = true;

        // Reiniciamos la velocidad vertical
        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        // Aplicamos el segundo impulso
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Debug.Log("¡Segundo salto ejecutado!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckGround(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckGround(collision);
    }

    void CheckGround(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    secondJumpUsed = false;
                    return;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}