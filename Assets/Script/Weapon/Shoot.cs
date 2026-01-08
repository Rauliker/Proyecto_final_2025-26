using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float speed = 20f;
    public GameObject bullet;
    public Transform firePoint;


    public void Disparar()
    {
        GameObject bala = Instantiate(bullet, firePoint.position, firePoint.rotation);

        Rigidbody rb = bala.GetComponent<Rigidbody>();

        rb.isKinematic = false;

        rb.linearVelocity = firePoint.forward * speed;

        Destroy(bala, 5f);
    }
}
