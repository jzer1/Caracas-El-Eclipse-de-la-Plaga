using UnityEngine;
using System.Collections.Generic;

public class ChunkTrigger : MonoBehaviour
{
    MapController mc;
    public GameObject targetMap;

    void Start()
    {
        mc = Object.FindFirstObjectByType<MapController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            mc.currentChunck = targetMap;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (mc.currentChunck == targetMap) 
            {
                mc.currentChunck = null;
            }
        }
    }


}
