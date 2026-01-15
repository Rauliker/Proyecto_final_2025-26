using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveData
{
    public int enemyCount;       
    public List<GameObject> SpawnPoints;

    public WaveData(int enemyCount, List<GameObject> SpawnPoints)
    {
        this.enemyCount = enemyCount;
        this.SpawnPoints = SpawnPoints;
    }
}