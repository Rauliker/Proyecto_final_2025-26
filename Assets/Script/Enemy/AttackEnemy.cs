using UnityEngine;
using System.Collections;

public class AttackEnemy : MonoBehaviour
{
    public Enemy enemy;     
    public Player player;   

    private bool atacando = false;
    private Coroutine ataqueCoroutine;

    void Start()
    {
        if (enemy != null && enemy.Jugador != null)
        {
            player = enemy.Jugador.GetComponent<Player>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !atacando)
        {
            atacando = true;
            ataqueCoroutine = StartCoroutine(AtacarCadaSegundo());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && atacando)
        {
            atacando = false;
            if (ataqueCoroutine != null)
            {
                StopCoroutine(ataqueCoroutine);
            }
        }
    }

    IEnumerator AtacarCadaSegundo()
    {
        while (atacando)
        {
            if (enemy == null || player == null)
                yield break;

            enemy.Attack(); 
            yield return player.RecibirDanio(enemy.dano, 0f); 

            yield return new WaitForSeconds(1f); 
        }
    }
}
