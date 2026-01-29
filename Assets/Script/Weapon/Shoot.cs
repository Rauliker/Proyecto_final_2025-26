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

    public int damage = 10;

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

    private Coroutine reloadCoroutine;



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


    private void Update()
    {
        ActualizarMensajeRecarga();
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

                // Realizar disparo
                ammunition_clip--;
                Vector3 baseDirection = GetShootDirection();
                for (int i = 0; i < bulletsPerShot; i++)
                {
                    float spreadFinal = player.apuntando ? 0f : spreadAngle;

                    Vector3 direccion = Quaternion.Euler(
                        Random.Range(-spreadFinal, spreadFinal),
                        Random.Range(-spreadFinal, spreadFinal),
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
        
        player.AnimationShoot(transform.GetComponent<Recoger>().tipoArma);
        StartCoroutine(CrearBalaDespuesDeDelay(direccion, 0.2f));
    }

    private IEnumerator CrearBalaDespuesDeDelay(Vector3 direccion, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject bala = Instantiate(bullet, firePoint.position, firePoint.rotation);
        bala.GetComponent<Bullet>().damage = damage;
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

        reloadCoroutine = StartCoroutine(RecargarCoroutine());
    }

    public void CancelarRecarga()
    {
        if (!isReloading)
            return;

        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }

        isReloading = false;
        textReload.enabled = false;

        Debug.Log("Recarga cancelada");
    }



    private IEnumerator RecargarCoroutine()
    {
        isReloading = true;
        isShooting = false;

        yield return new WaitForSeconds(reloadTime);

        int balasFaltantes = clipSize - ammunition_clip;
        int balasACargar = Mathf.Min(balasFaltantes, ammo);

        ammunition_clip += balasACargar;
        ammo -= balasACargar;

        isReloading = false;
        reloadCoroutine = null;

        textReload.enabled = false;
        textAmmo.text = $"{ammunition_clip}/{ammo}";
    }


    Vector3 GetShootDirection()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        return ray.direction;
    }

    void ActualizarMensajeRecarga()
    {
        if (isReloading)
        {
            textReload.text = LocalizationManager.Instance.GetTranslation("RECARGANDO");
            textReload.enabled = true;
            return;
        }

        if (ammunition_clip <= reloadTextAmmo && ammo > 0)
        {
            textReload.text = LocalizationManager.Instance.GetTranslation("RECARGAR_ARMA");
            textReload.enabled = true;
            return;
        }

        textReload.enabled = false;
    }

}
