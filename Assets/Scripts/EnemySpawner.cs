using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; // List<T> requiere using System.Collections.Generic
        public int waveQuota;
        public float apawnInterval;
        public int spawnCount;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int currentWaveCount;


    [Header("Spawner Attributes")]
    float spawnTimer;
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached = false;
    public float waveInterval;


    [Header("Spawn positions")]
    public List<Transform> spawnPoints;

    Transform player;

    void Start()
    {
        player = FindAnyObjectByType<PlayerNivel>().transform;
        CalculateWaveQuota();
    }


    void Update()
    {
        if (currentWaveCount < waves.Count &&
    waves[currentWaveCount].spawnCount >= waves[currentWaveCount].waveQuota)
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= waves[currentWaveCount].apawnInterval)
        {
            SpawnEnemies();
            spawnTimer = 0f;
        }
    }

    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)     
        {
            currentWaveCount++;
                    CalculateWaveQuota();

        }
    }


    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGourp in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGourp.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }


    //<sumaey>


    void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount< waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            foreach(var enemyGoup in waves[currentWaveCount].enemyGroups)
            {


                if (enemyGoup.spawnCount< enemyGoup.enemyCount)
                {
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                    Instantiate(enemyGoup.enemyPrefab, player.position + spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);

                    enemyGoup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;   
                    enemiesAlive++;
                }
            }
        }

        if(enemiesAlive< maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }

}


