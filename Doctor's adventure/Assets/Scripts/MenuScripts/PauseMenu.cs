using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  //Menü felvétele
    
    private bool isPaused = false;  //A megállítás nyomonkövetésére szolgáló bool

    private bool inOtherMenu = false;   //Igaz/hamis arra az esetre hogyha nem a menű fő része lenne megnyitva
    void Update()
    {
        if (!DialogueManager.prologue) return;  //Amig a dialógus megy, ne lehessen használni a különböző billentyűket
        
        //Escape gomb megnyomására a játék folytatása vagy megállítása
        if (Input.GetKeyDown(KeyCode.Escape) && !inOtherMenu) 
        {
            if (isPaused) Resume(null);
            else Pause();
        }
    }

    /// <summary>
    /// A játék folytatása, menü bezárása
    /// </summary>
    public void Resume(GameObject menu)
    {
        Time.timeScale = 1f;
        isPaused = false;
        inOtherMenu = false;
        pauseMenuUI.SetActive(false);
        if (menu != null) menu.SetActive(false);
    }

    /// <summary>
    /// A megadott menüpont megnyitása
    /// </summary>
    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
        pauseMenuUI.SetActive(false);
        inOtherMenu = true;
    }

    /// <summary>
    /// A megadott menüpont bezárása
    /// </summary>
    public void BackMenu(GameObject menu)
    {
        menu.SetActive(false);
        pauseMenuUI.SetActive(true);
        inOtherMenu = false;
    }

    /// <summary>
    /// Main menü betöltése
    /// </summary>
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Játék megállítása
    /// </summary>
    void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuUI.SetActive(true);
    }
}
