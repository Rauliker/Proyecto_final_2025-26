using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement3D : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip audioWalk;
    public float walkSoundInterval = 0.4f; // Tiempo entre pasos
    public float stopDelay = 0.3f; // Tiempo que espera antes de detener los pasos al dejar de moverse
    private float walkTimer = 0f;
    private float stopTimer = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        walkTimer = 0f; // Para que el primer paso se reproduzca inmediatamente
    }

    public void Move(Vector2 input, bool jump, bool isSprinting)
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float currentSpeed = isSprinting ? sprintSpeed : speed;

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Manejo de sonido de pasos
        if (isGrounded && move.magnitude > 0.1f)
        {
            // Reinicia temporizador de inactividad
            stopTimer = stopDelay;

            // Reproduce paso inmediatamente si el temporizador llegó a 0
            walkTimer -= Time.deltaTime;
            if (walkTimer <= 0f)
            {
                audioSource.PlayOneShot(audioWalk);
                walkTimer = walkSoundInterval;
            }
        }
        else
        {
            // Cuenta regresiva de inactividad antes de reiniciar el temporizador
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0f)
            {
                walkTimer = 0f; // Se reinicia para el próximo movimiento
            }
        }

        // Saltar
        if (jump && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
