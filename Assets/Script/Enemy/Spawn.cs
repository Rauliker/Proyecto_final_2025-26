using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> enemy = new List<GameObject>();

    void Start()
    {
        int randomPoint = Random.Range(0, spawnPoints.Count);
        int randomEnemy = Random.Range(0, enemy.Count);

        Instantiate(enemy[randomEnemy],
                    spawnPoints[randomPoint].transform.position,
                    Quaternion.identity);
    }


    void Update()
    {
        
    }
}
