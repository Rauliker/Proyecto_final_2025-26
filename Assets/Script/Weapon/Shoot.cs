using System.Collections;
using TMPro;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float speed = 20f;
    public float bulletGravity = 9.8f;

    public Player player;

    public Camera playerCamera;
    public float maxDistance = 1000f;

    [Header("Munición")]
    public int clipSize = 10;
    public int ammunition_clip;
    public int ammo = 100;
    public int maxAmmo = 200;
    public int reloadTextAmmo = 3;
    public TextMeshProUGUI textAmmo;

    [Header("Cadencia")]
    public float fireRate = 0.5f;

    [Header("Modo de Bala")]
    public FireMode fireMode = FireMode.Single;
    public int bulletsPerShot = 1;
    public float spreadAngle = 5f;

    [Header("Recarga")]
    public float reloadTime = 2f;
    private bool isReloading = false;
    public TextMeshProUGUI textReload;

    public GameObject bullet;
    public Transform firePoint;

    private float nextFireTime = 0f;
    private bool isShooting = false;

    public enum FireMode
    {
        Single,
        Multiple
    }

    public void Start()
    {
        ammunition_clip = clipSize;
        textAmmo.text = $"{ammunition_clip}/{ammo}";
    }

    public void StartShooting()
    {
        if (isReloading)
            return;

        if (fireMode == FireMode.Single)
        {
            Disparar();
        }
        else
        {
            isShooting = true;
            StartCoroutine(ContinuousShooting());
        }
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    private IEnumerator ContinuousShooting()
    {
        while (isShooting)
        {
            if (Time.time >= nextFireTime)
            {
                if (isReloading)
                {
                    isShooting = false;
                    yield break;
                }

                if (ammunition_clip <= 0)
                {
                    Recargar();
                    isShooting = false;
                    yield break;
                }

                // Mostrar advertencia de recarga
                if (ammunition_clip <= 3)
                {
                    string text = LocalizationManager.Instance.GetTranslation("RECARGAR_ARMA");
                    textReload.text = text;
                    textReload.enabled = true;
                }
                else
                {
                    textReload.enabled = false;
                }

                // Realizar disparo
                ammunition_clip--;
                Vector3 baseDirection = GetShootDirection();
                for (int i = 0; i < bulletsPerShot; i++)
                {
                    Vector3 direccion = Quaternion.Euler(
                        Random.Range(-spreadAngle, spreadAngle),
                        Random.Range(-spreadAngle, spreadAngle),
                        0f
                    ) * baseDirection;

                    DispararBala(direccion);
                }

                ActualizarTexto();
                nextFireTime = Time.time + fireRate;
            }

            yield return null;
        }
    }

    public void Disparar()
    {
        if (isReloading)
            return;

        if (ammunition_clip <= 3)
        {
            string text = LocalizationManager.Instance.GetTranslation("RECARGAR_ARMA");
            textReload.text = text;
            textReload.enabled = true;
        }

        if (ammunition_clip <= 0)
        {
            Recargar();
            return;
        }

        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;

        if (fireMode == FireMode.Single)
        {
            ammunition_clip--;
            Vector3 baseDirection = GetShootDirection();
            DispararBala(baseDirection);
            ActualizarTexto();
        }
        // El disparo múltiple ahora se maneja en ContinuousShooting
    }

    public void ActualizarTexto()
    {
        textAmmo.text = $"{ammunition_clip}/{ammo}";
    }

    void DispararBala(Vector3 direccion)
    {
        player.AnimationShoot();
        StartCoroutine(CrearBalaDespuesDeDelay(direccion, 0.2f));
    }

    private IEnumerator CrearBalaDespuesDeDelay(Vector3 direccion, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject bala = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Rigidbody rb = bala.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = false;
        rb.linearVelocity = direccion * speed;

        Destroy(bala, 5f);
    }

    public void Recargar()
    {
        if (isReloading || ammunition_clip == clipSize || ammo <= 0)
            return;

        StartCoroutine(RecargarCoroutine());
    }

    private IEnumerator RecargarCoroutine()
    {
        isReloading = true;
        isShooting = false; // Detener disparo durante recarga
        Debug.Log("Recargando...");

        textReload.text = LocalizationManager.Instance.GetTranslation("RECARGANDO");
        textReload.enabled = true;

        yield return new WaitForSeconds(reloadTime);

        int balasFaltantes = clipSize - ammunition_clip;
        int balasACargar = Mathf.Min(balasFaltantes, ammo);

        ammunition_clip += balasACargar;
        ammo -= balasACargar;

        isReloading = false;

        textReload.enabled = false;
        Debug.Log($"Recarga completa! Balas en cargador: {ammunition_clip}, Munición total: {ammo}");

        textAmmo.text = $"{ammunition_clip}/{ammo}";
    }

    Vector3 GetShootDirection()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        return ray.direction;
    }
}