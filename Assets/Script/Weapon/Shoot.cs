using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float speed = 20f;
    public float bulletGravity = 9.8f;

    [Header("Cadencia")]
    public float fireRate = 0.3f;

    [Header("Modo de Bala")]
    public FireMode fireMode = FireMode.Single;
    public int bulletsPerShot = 3;
    public float spreadAngle = 5f;

    public GameObject bullet;
    public Transform firePoint;

    private float nextFireTime = 0f;

    public enum FireMode
    {
        Single,
        Multiple
    }


    public void Disparar()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireRate;

        if (fireMode == FireMode.Single)
        {
            DispararBala(firePoint.forward);
        }
        else
        {
            DisparoMultiple();
        }
    }

    void DispararBala(Vector3 direccion)
    {
        GameObject bala = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Rigidbody rb = bala.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = false;

        rb.linearVelocity = direccion * speed;
        rb.AddForce(Vector3.down * bulletGravity, ForceMode.Acceleration);

        Destroy(bala, 5f);
    }


    void DisparoMultiple()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector3 direccion = Quaternion.Euler(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0f
            ) * firePoint.forward;

            DispararBala(direccion);
        }
    }
}
