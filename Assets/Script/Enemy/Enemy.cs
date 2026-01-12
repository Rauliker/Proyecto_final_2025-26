using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int vida = 100;
    public GameObject Jugador;
    public float velocidad = 5f;

    void Update()
    {
        SeguirJugador();
    }

    void SeguirJugador()
    {
        if (Jugador != null)
        {
            // Mueve al enemigo hacia la posición del jugador
            transform.position = Vector3.MoveTowards(
                transform.position,
                Jugador.transform.position,
                velocidad * Time.deltaTime
            );

            Vector3 direccion = Jugador.transform.position - transform.position;
            if (direccion != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direccion),
                    5f * Time.deltaTime
                );
            }
        }
    }

    public void RecibirDanio(int danio)
    {
        vida -= danio;
        Debug.Log("Diana golpeada. Vida restante: " + vida);

        if (vida <= 0)
        {
            Destruir();
        }
    }

    void Destruir()
    {
        Debug.Log("Diana destruida");
        Destroy(gameObject);
    }
}
