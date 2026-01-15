using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public static Spawn Instance;

    public GameObject Jugador;
    public TextMeshProUGUI WaveTexto;

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
        if (ActualWave <= 0 || ActualWave > waves.Count)
        {
            Debug.LogError($"Ola {ActualWave} no existe. Verifica que tienes al menos {ActualWave} olas definidas.");
            return;
        }

        WaveData currentWave = waves[ActualWave - 1];

        if (currentWave.SpawnPoints == null || currentWave.SpawnPoints.Count == 0)
        {
            Debug.LogWarning($"No hay puntos de spawn definidos para la ola {ActualWave}.");
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

    private void UpdateWaveText()
    {
        if (WaveTexto != null)
        {
            WaveTexto.text = LocalizationManager.Instance.GetTranslation("OLEADA") + " " + ActualWave.ToString();
        }
    }

    public void NextWave()
    {
        if (ActualWave < waves.Count)
        {
            ActualWave++;
        }
        SpawnAllEnemiesInWave();
        UpdateWaveText();
    }
}