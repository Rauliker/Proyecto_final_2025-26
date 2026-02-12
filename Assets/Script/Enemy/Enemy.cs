using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float vida = 1;
    public GameObject Jugador;
    public int points = 10;
    public int dano = 10;
    public Animator animator;

    private NavMeshAgent agente;
    private Rigidbody rb;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (agente != null)
            agente.speed = 5f;
    }

    void Update()
    {
        SeguirJugador();
    }

    void SeguirJugador()
    {
        if (Jugador != null && agente != null && agente.isOnNavMesh)
        {
            Vector3 destino = Jugador.transform.position;
            destino.y = transform.position.y;
            agente.SetDestination(destino);
        }
    }

    public void RecibirDanio(int danio)
    {
        vida -= danio;
        Debug.Log("Enemigo golpeado. Vida restante: " + vida);

        if (vida <= 0)
            Destruir();
    }

    void Destruir()
    {
        Debug.Log("Enemigo destruido. Otorgando " + points + " puntos.");

        Player playerScript = Jugador?.GetComponent<Player>();
        if (playerScript != null)
            playerScript.AddPoints(points);
        
        if (0== HasAnyAliveEnemies())
            Spawn.Instance.NextWave();

        Destroy(gameObject);
    }

    private int HasAnyAliveEnemies()
    {
        var enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.InstanceID);
        return enemies.Length - 1;
    }

    // Método de ataque
    public void Attack(Player target)
    {
        if (animator != null)
            animator.SetTrigger("a");
    }
}
