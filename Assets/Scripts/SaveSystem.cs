using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath(int slot)
    {
        return Application.persistentDataPath + "/saveSlot" + slot + ".json";
    }

    public static void Guardar(PlayerData data, int slot)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath(slot), json);
        Debug.Log("Juego guardado en slot " + slot);
    }

    public static PlayerData Cargar(int slot)
    {
        string path = savePath(slot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }
        else
        {
            Debug.Log("No se encontró guardado en el slot " + slot);
            return null;
        }
    }
}
