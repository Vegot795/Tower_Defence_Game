using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButHandler : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject NewGamePanel;
    public GameObject SettPanel;

    void Start()
    {
        MainMenuPanel.SetActive(true);
        SettPanel.SetActive(false);
        NewGamePanel.SetActive(false);
    }

    public void StartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1");
    }
    public void QuitGame()
    {
        Application.Quit(); 
    }
    
}
