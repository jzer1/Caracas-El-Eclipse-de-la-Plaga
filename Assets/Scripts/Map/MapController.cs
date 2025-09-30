using UnityEngine;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    [Header("Chunks")]
    public List<GameObject> terrainChunks; // Prefabs disponibles
    public GameObject player;
    public float checkerRadius = 0.5f;
    public int chunkSize = 10; // Tamaño del chunk (asumimos que es cuadrado)
    public float CheckerRadius = 5f; 



    [Header("Optimization")]
    public List<GameObject> spawnedChunks = new List<GameObject>();
    public float maxOpDist = 40f; // Distancia máxima para mantener activo un chunk
    public float optomizerCooldownDur = 0.5f; // Cada cuánto tiempo se optimiza

    float optomizerCooldown;
    GameObject latestChunk;

    // Grid para controlar duplicados
    HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();


    void Start()
    {
        //genera el primer chunk donde aparece el jugador
        Vector3Int startGrid = GridPos(player.transform.position);
        SpawnChunk(startGrid * chunkSize);

    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    //convierte una posición en la grilla de chunks
    Vector3Int GridPos(Vector3 position)
    {
        return new Vector3Int(
            Mathf.FloorToInt(position.x / chunkSize),
            Mathf.FloorToInt(position.y / chunkSize),
            0);
    }

    void ChunkChecker()
    {
        Vector3 playerPos = player.transform.position;
        Vector3Int currentGrid = GridPos(playerPos);

        for (int x=-1; x<= 1; x++)
        {

            for(int y=-1; y <= 1; y++)
            {

                Vector3Int checkGrid =currentGrid + new Vector3Int(x, y, 0);
                Vector3 spawnPos = checkGrid * chunkSize;

                if(!occupiedPositions.Contains(checkGrid))
                {
                    SpawnChunk(spawnPos);
                }
            }
        }

    }

    void SpawnChunk(Vector3 position)
    {
        Vector3Int gridPos = GridPos(position);

        if(occupiedPositions.Contains(gridPos)) return;

        int rend = Random.Range(0,terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rend], position, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
        occupiedPositions.Add(gridPos);
    }

    void ChunkOptimizer()
    {
        optomizerCooldown -= Time.deltaTime;
        if (optomizerCooldown > 0f) return;

        optomizerCooldown = optomizerCooldownDur;

        foreach (GameObject chunk in spawnedChunks)
        {
            if(!chunk) continue;
            float dist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if(dist>maxOpDist)
                chunk.SetActive(false);
            else
                chunk.SetActive(true);
        }
    }
}
