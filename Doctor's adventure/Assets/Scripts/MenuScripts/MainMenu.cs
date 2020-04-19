using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;   //A menü fő lapja

    /// <summary>
    /// Új játék kezdése
    /// </summary>
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// A megadott menüpont megnyitása
    /// </summary>
    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    /// <summary>
    /// A megadott menüpont bezárása, majd a főmenű megnyitása
    /// </summary>
    public void BackMenu(GameObject menu)
    {
        menu.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    /// <summary>
    /// Az alkalmazás bezárása
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
