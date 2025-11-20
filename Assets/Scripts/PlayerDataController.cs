using System.Collections.Generic;
using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    public PlayerNivel playerNivel; // Aquí referenciamos tu script
    public List<string> unlockedMaps = new List<string>();
    public List<string> unlockedCharacters = new List<string>();

    public void Guardar(int slot)
    {
        // Tomamos los datos del PlayerNivel
        PlayerData data = new PlayerData
        {
            fragments = playerNivel.fragments,
            unlockedMaps = unlockedMaps,
            unlockedCharacters = unlockedCharacters
        };

        SaveSystem.Guardar(data, slot);
    }

    public void Cargar(int slot)
    {
        PlayerData data = SaveSystem.Cargar(slot);
        if (data != null)
        {
            playerNivel.fragments = data.fragments; // Aplicamos los fragmentos
            unlockedMaps = data.unlockedMaps;
            unlockedCharacters = data.unlockedCharacters;

            // Aquí puedes actualizar UI o personajes desbloqueados
        }
    }
}
