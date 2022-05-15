using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        PlayerPrefs.SetString("GAME_MODE", "new");
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }

    public void ContinueGame()
    {
        if (!SaveSystem.SaveExists("player") || !SaveSystem.SaveExists("inventory"))
        {
            Debug.Log("Save find not found!");
            return;
        }
        PlayerPrefs.SetString("GAME_MODE", "continue");
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
