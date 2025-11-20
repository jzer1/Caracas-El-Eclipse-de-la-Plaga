using System.Collections.Generic;
[System.Serializable]
public class PlayerData
{
    public int fragments; // Dinero/fragmentos
    public int level; // Nivel si quieres guardar progreso
    public List<string> unlockedMaps; // Nombres de escenas desbloqueadas
    public List<string> unlockedCharacters; // Personajes desbloqueados
}
