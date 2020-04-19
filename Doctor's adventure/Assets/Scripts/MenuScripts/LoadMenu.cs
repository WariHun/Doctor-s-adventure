using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    private Button[] buttons;   //Gomb tömb a menüben lévő mentés gomboknak
    private int currentScene;   //Az adott scene mentése
    public static int saveslot = 0; //Save slot

    void Awake()
    {
        buttons = this.GetComponentsInChildren<Button>();   //Gombok megkeresése
    }
    
    void Start()
    {
        Scan();
        currentScene = SceneManager.GetActiveScene().buildIndex;    //A jelenlegi scene-nek az indexét eltárolja késöbbre
    }

    /// <summary>
    /// Megvizsgálja a mentéseket, ha nem talál mentést az adott slot számával, akkor használhatatlanná teszi a gombot
    /// </summary>
    public void Scan()
    {
        for (int i = 0; i < buttons.Length-1; i++)
        {
            string path = Application.persistentDataPath + "/player" + (i+1).ToString() + ".sav";
            if (!File.Exists(path)) buttons[i].interactable = false;
            else buttons[i].interactable = true;
        }
    }

    /// <summary>
    /// Ha a scene beépített indexe eltér a főmenű indexétől,
    /// betölti a mentést, más esetben elmenti a slot számot késöbbi művelethez
    /// </summary>
    public void LoadGame(int slot)
    {
        if (currentScene !=0) SaveLoadScript.Load(slot);
        else saveslot = slot;
    }
}
