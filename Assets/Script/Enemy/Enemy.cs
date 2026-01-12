using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int vida = 100;
    public GameObject Jugador;

    private NavMeshAgent agente;
    private Rigidbody rb;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        // Activamos la gravedad del Rigidbody
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Evita que se caiga girando
        agente.speed = 5f;
    }

    void Update()
    {
        SeguirJugador();
    }

    void SeguirJugador()
    {
        if (Jugador != null && agente.isOnNavMesh)
        {
            // Calculamos destino solo en XZ
            Vector3 destino = Jugador.transform.position;
            destino.y = transform.position.y;
            agente.SetDestination(destino);
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
