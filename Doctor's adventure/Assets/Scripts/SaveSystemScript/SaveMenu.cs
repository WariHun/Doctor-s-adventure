using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    /// <summary>
    /// A játék mentése az adott slotra
    /// </summary>
    public void SaveGame(int slot)
    {
        SaveLoadScript.Save(slot);
    }
}
