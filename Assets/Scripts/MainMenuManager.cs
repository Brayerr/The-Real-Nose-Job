using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject welcomePanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject levelSelectPanel;

    public void StartHereButton()
    {
        mainMenuPanel.SetActive(true);
        welcomePanel.SetActive(false);
    }

    public void StartANewGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void LevelSelectButton()
    {
        levelSelectPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void LevelSelectBackButton()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
