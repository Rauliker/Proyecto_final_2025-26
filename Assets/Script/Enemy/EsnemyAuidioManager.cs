using UnityEngine;

public class ZombieAmbienceManager : MonoBehaviour
{
    private AudioSource ambienceSource;
    public Transform jugador;
    public float radio = 20f;

    void Awake()
    {
        ambienceSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");

        int cercanos = 0;

        foreach (GameObject z in zombies)
        {
            float distancia = Vector3.Distance(
                z.transform.position,
                jugador.position
            );

            if (distancia < radio)
                cercanos++;
        }

        float volumenObjetivo = Mathf.Clamp01(cercanos / 10f);
        ambienceSource.volume = volumenObjetivo;
    }
}
