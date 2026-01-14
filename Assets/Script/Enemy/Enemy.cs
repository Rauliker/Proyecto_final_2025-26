using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int vida = 100;
    public GameObject Jugador;

    private NavMeshAgent agente;
    private Rigidbody rb;

    public int points = 10;

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
        Debug.Log("Enemigo destruido. Otorgando " + points + " puntos.");

        // Sumar puntos al jugador
        Player playerScript = Jugador?.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.AddPoints(points); 
        }
        Spawn.Instance?.SpawnEnemy();

        Destroy(gameObject);
    }
}
