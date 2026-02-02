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

    private AudioSource audioSource;
    public AudioClip ausioWalk;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        audioSource = transform.GetComponent<AudioSource>();
    }

    public void Move(Vector2 input, bool jump, bool isSprinting)
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float currentSpeed = isSprinting ? sprintSpeed : speed;

        Vector3 move = transform.right * input.x + transform.forward * input.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (jump && isGrounded) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        audioSource.PlayOneShot(ausioWalk);
    }
}