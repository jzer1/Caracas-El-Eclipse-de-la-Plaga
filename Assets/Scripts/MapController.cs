using UnityEngine;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    [Header("Chunks")]
    public List<GameObject> terrainChunks; // Prefabs disponibles
    public GameObject player;
    public float checkerRadius = 0.5f;
    public LayerMask terrainMask;
    public GameObject currentChunck;

    MovimientoPlayer pm;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks = new List<GameObject>();
    GameObject latestChunk;
    public float maxOpDist = 20f;
    float opDist;
    float optomizerCooldown;
    public float optomizerCooldownDur = 0.5f;

    // Guardamos posiciones ocupadas para no duplicar
    HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    // Diccionario de direcciones → hijos del prefab
    Dictionary<Vector2Int, string> dirToChild;

    void Start()
    {
        pm = Object.FindFirstObjectByType<MovimientoPlayer>();

        dirToChild = new Dictionary<Vector2Int, string>
        {
            { Vector2Int.right, "Right" },
            { Vector2Int.left, "Left" },
            { Vector2Int.up, "Up" },
            { Vector2Int.down, "Down" },
            { new Vector2Int(1,1), "Right Up" },
            { new Vector2Int(1,-1), "Right Down" },
            { new Vector2Int(-1,1), "Left Up" },
            { new Vector2Int(-1,-1), "Left Down" }
        };
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {
        if (!currentChunck) return;

        Vector2Int dir = pm.MovementDir;

        if (dir != Vector2Int.zero && dirToChild.ContainsKey(dir))
        {
            string childName = dirToChild[dir];
            Transform child = currentChunck.transform.Find(childName);

            if (child && !Physics2D.OverlapCircle(child.position, checkerRadius, terrainMask))
            {
                SpawnChunk(child.position);
            }

            // 🔥 Extra: si es diagonal, también revisamos los ejes
            if (dir.x != 0 && dirToChild.ContainsKey(new Vector2Int(dir.x, 0)))
            {
                string sideChild = dirToChild[new Vector2Int(dir.x, 0)];
                Transform side = currentChunck.transform.Find(sideChild);
                if (side && !Physics2D.OverlapCircle(side.position, checkerRadius, terrainMask))
                {
                    SpawnChunk(side.position);
                }
            }
            if (dir.y != 0 && dirToChild.ContainsKey(new Vector2Int(0, dir.y)))
            {
                string sideChild = dirToChild[new Vector2Int(0, dir.y)];
                Transform side = currentChunck.transform.Find(sideChild);
                if (side && !Physics2D.OverlapCircle(side.position, checkerRadius, terrainMask))
                {
                    SpawnChunk(side.position);
                }
            }
        }
    }

    void SpawnChunk(Vector3 position)
    {
        // Evita duplicados
        if (occupiedPositions.Contains(position)) return;

        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], position, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
        occupiedPositions.Add(position);
    }

    void ChunkOptimizer()
    {
        optomizerCooldown -= Time.deltaTime;
        if (optomizerCooldown > 0f) return;

        optomizerCooldown = optomizerCooldownDur;

        foreach (GameObject chunk in spawnedChunks)
        {
            if (!chunk) continue;

            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);

            if (opDist > maxOpDist)
                chunk.SetActive(false);
            else
                chunk.SetActive(true);
        }
    }
}
