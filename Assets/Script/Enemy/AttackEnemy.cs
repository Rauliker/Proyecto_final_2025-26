using UnityEngine;
using System.Collections;

public class AttackEnemy : MonoBehaviour
{
    public Enemy enemy;
    public Player player;
    public GameObject hitbox;

    private bool isAttacking = false;
    private Coroutine attackCoroutine;

    void Start()
    {
        if (player == null && enemy != null && enemy.Jugador != null)
        {
            player = enemy.Jugador.GetComponent<Player>();
        }

        if (hitbox != null)
            hitbox.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;

            if (hitbox != null && !hitbox.activeSelf)
                hitbox.SetActive(true);

            attackCoroutine = StartCoroutine(AttackEverySecond());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isAttacking)
        {
            isAttacking = false;

            if (hitbox != null && hitbox.activeSelf)
                hitbox.SetActive(false);

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    IEnumerator AttackEverySecond()
    {
        while (isAttacking)
        {
            if (enemy == null || player == null)
                yield break;

            enemy.Attack(player);

            yield return new WaitForSeconds(1f);
        }
    }
}
