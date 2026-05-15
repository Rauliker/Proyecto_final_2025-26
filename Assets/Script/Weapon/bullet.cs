using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public Vector3 direction;
    public ParticleSystem bloodPrefab;

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);

        if (other.CompareTag("Enemy"))
        {
            HitboxDamage hitbox = other.GetComponent<HitboxDamage>();

            if (hitbox != null)
            {
                // Efecto de sangre
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Quaternion rot = Quaternion.LookRotation(-direction);

                ParticleSystem bloodInstance = Instantiate(bloodPrefab, hitPoint, rot);
                bloodInstance.Play();

                Destroy(bloodInstance.gameObject,
                    bloodInstance.main.duration + bloodInstance.main.startLifetime.constantMax);

                hitbox.Recibirdanio(damage);
            }

            Destroy(gameObject);
        }
    }
}