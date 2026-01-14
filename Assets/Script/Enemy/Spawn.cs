using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public static Spawn Instance;

    public GameObject Jugador; 
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> enemyPrefabs = new List<GameObject>(); 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnEnemy(); 
    }

    public void SpawnEnemy()
    {
        if (spawnPoints.Count == 0 || enemyPrefabs.Count == 0 || Jugador == null)
        {
            Debug.LogWarning("No se puede spawnear enemigo: faltan referencias (spawnPoints, enemyPrefabs o Jugador).");
            return;
        }

        int randomPoint = Random.Range(0, spawnPoints.Count);
        int randomEnemy = Random.Range(0, enemyPrefabs.Count);

        GameObject nuevoEnemigo = Instantiate(
            enemyPrefabs[randomEnemy],
            spawnPoints[randomPoint].transform.position,
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