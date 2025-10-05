using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : MonoBehaviour
{
    [System.Serializable]
    public class Drop
    {
        public string itemName;
        public GameObject itemPrefab;
        [UnityEngine.Range(0f, 100f)] public float dropChance; // % de probabilidad
        public int minAmount = 1; // Cantidad mínima
        public int maxAmount = 1; // Cantidad máxima
    }

    [Header("Lista de posibles drops")]
    public List<Drop> drops;

    // Llamar este método en Enemy.Die()
    public void DropItem()
    {
        foreach (Drop drop in drops)
        {
            float roll = UnityEngine.Random.Range(0f, 100f);

            if (roll <= drop.dropChance)
            {
                int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);

                for (int i = 0; i < amount; i++)
                {
                    Vector3 spawnPos = transform.position + new Vector3(
                        Random.Range(-0.5f, 0.5f),
                        Random.Range(-0.5f, 0.5f),
                        0f);

                    Instantiate(drop.itemPrefab, spawnPos, Quaternion.identity);
                }

                Debug.Log($"Drop: {drop.itemName} x{amount}");
            }
        }
    }
}
