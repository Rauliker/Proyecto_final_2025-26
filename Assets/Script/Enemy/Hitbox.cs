using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Enemy enemy;

    void Start()
    {
        if (enemy == null)
        {
            enemy = GetComponentInParent<Enemy>();
        }

        if (enemy == null)
        {
            enemy = transform.parent?.GetComponent<Enemy>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                StartCoroutine(player.RecibirDanio(enemy.dano, 0f));
            }
        }
    }
}
