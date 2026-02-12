using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public static Spawn Instance;

    public GameObject Jugador;
    public TextMeshProUGUI WaveTexto;

    public float aumentoVida = 5f;
    public float vidaBase = 1f;

    private int ActualWave;
    public List<WaveData> waves = new List<WaveData>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ActualWave = 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnAllEnemiesInWave();
        UpdateWaveText();
    }

    public void SpawnAllEnemiesInWave()
    {
        int waveIndex = GetCurrentWaveIndex();

        if (waves.Count == 0)
        {
            Debug.LogError("No hay oleadas definidas.");
            return;
        }

        WaveData currentWave = waves[waveIndex];

        if (currentWave.SpawnPoints == null || currentWave.SpawnPoints.Count == 0)
        {
            Debug.LogWarning($"No hay puntos de spawn definidos para la oleada {ActualWave}.");
            return;
        }

        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("No hay prefabs de enemigos definidos.");
            return;
        }

        if (Jugador == null)
        {
            Debug.LogWarning("Jugador no asignado.");
            return;
        }

        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            int randomPointIndex = Random.Range(0, currentWave.SpawnPoints.Count);
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Count);

            GameObject nuevoEnemigo = Instantiate(
                enemyPrefabs[randomEnemyIndex],
                currentWave.SpawnPoints[randomPointIndex].transform.position,
                Quaternion.identity
            );
            Enemy enemyScript = nuevoEnemigo.GetComponent<Enemy>();
            int aumento = Mathf.Max(1, Mathf.RoundToInt(vidaBase * aumentoVida));
            enemyScript.vida += aumento;
            if (enemyScript != null)
            {
                enemyScript.Jugador = Jugador;
            }
            else
            {
                Debug.LogError("El prefab instanciado no tiene el componente Enemy.");
            }
        }
    }

    private int GetCurrentWaveIndex()
    {
        if (waves.Count == 0) return 0;

        if (ActualWave <= waves.Count)
        {
            return ActualWave - 1;
        }
        else
        {
            return waves.Count - 1;
        }
    }

    private void UpdateWaveText()
    {
        if (WaveTexto != null)
        {
            string waveText = LocalizationManager.Instance.GetTranslation("OLEADA") + " " + ActualWave.ToString();

            WaveTexto.text = waveText;
        }
    }

    public void NextWave()
    {
        ActualWave++;
        aumentoVida += 0.1f;

        SpawnAllEnemiesInWave();
        UpdateWaveText();
    }
}