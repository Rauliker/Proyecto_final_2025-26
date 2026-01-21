using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    public Enemy enemy;

    public Collider SphereDetection;

    public void OnTriggerEnter(Collider other)
    {
        if (enemy == null)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {

            enemy.Attack();
        }
    }
}
