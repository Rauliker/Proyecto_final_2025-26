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

    private bool moviendo = true;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        if (agente != null)
            agente.speed = 5f;
    }

    void Update()
    {
        if (moviendo)
            SeguirJugador();
    }
    public void DetenerOContinuar()
    {
        moviendo = !moviendo;

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

        if (HasAnyAliveEnemies() == 0)
            Spawn.Instance.NextWave();

        Destroy(gameObject);
    }

    private int HasAnyAliveEnemies()
    {
        var enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.InstanceID);
        return enemies.Length - 1;
    }

    public void Attack(Player target)
    {
        if (animator != null)
            animator.SetTrigger("a");
    }
}