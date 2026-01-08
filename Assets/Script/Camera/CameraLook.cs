using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 80f;

    // Referencia al Transform del cuerpo del jugador para rotarlo junto con la cámara
    public Transform playerBody;

    // Variable para almacenar la rotación acumulada en el eje X (arriba/abajo)
    float xRotation = 0f;

    void Start()
    {
        // Bloquea el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;

        // Oculta el cursor para que no sea visible mientras se juega
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplica la rotación vertical solo a la cámara
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Aplica la rotación horizontal al cuerpo del jugador (para girar todo el jugador)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}