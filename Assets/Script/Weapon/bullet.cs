using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public Vector3 direction;

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);

        // Enemy
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
                enemy.RecibirDanio(damage);
        }
        Destroy(gameObject);
    }
}