using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.RecibirDanio(damage);
            }
        }

        Destroy(gameObject);
    }
}
