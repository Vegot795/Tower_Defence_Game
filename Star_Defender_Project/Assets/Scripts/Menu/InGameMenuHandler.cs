using UnityEngine;

public class InGameMenuHandler : MonoBehaviour
{
    public void StartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        Debug.Log("Starting Level 1");
    }

    public void QuitGame()
    {
        UnityEngine.Application.Quit();
        Debug.Log("Quitting Game");
    }

    public void ShowMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Debug.Log("Returning to Main Menu");
    }
}

