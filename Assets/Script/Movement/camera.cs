using UnityEngine;
public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    void Update()
    {
        // Movimiento con WASD (en plano XZ)
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) moveZ += 1f;
        if (Input.GetKey(KeyCode.S)) moveZ -= 1f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;
        if (Input.GetKey(KeyCode.A)) moveX -= 1f;

        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.Self);

        // Rotación con flechas (X e Y)
        float rotX = 0f;
        float rotY = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) rotX -= 1f; // mirar hacia abajo/arriba
        if (Input.GetKey(KeyCode.DownArrow)) rotX += 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) rotY -= 1f; // girar izquierda/derecha
        if (Input.GetKey(KeyCode.RightArrow)) rotY += 1f;

        Vector3 rotation = new Vector3(rotX, rotY, 0f) * rotationSpeed * Time.deltaTime;
        transform.eulerAngles += rotation;
    }
}

