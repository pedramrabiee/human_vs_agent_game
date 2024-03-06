using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Animator newPlayerButton;
    public Animator resumeGameButton;
    public GameObject newPlayerPanel;
    public GameObject resumeGamePanel;
    public Animator mainMenu;

    public void Awake()
    {
        Time.timeScale = 1;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("SceneManager");
    }

    public void openNewPlayerPanel()
    {
        newPlayerPanel.SetActive(true);
        mainMenu.SetBool("isHidden", true);
        newPlayerButton.SetBool("isHidden", false);
    }

    public void openResumeGamePanel()
    {
        resumeGamePanel.SetActive(true);
        mainMenu.SetBool("isHidden", true);
        resumeGameButton.SetBool("isHidden", false);
    }

    public void closeNewPlayerPanel()
    {
        mainMenu.SetBool("isHidden", false);
        newPlayerButton.SetBool("isHidden", true);
        newPlayerPanel.SetActive(false);

    }

    public void closeResumeGamePanel()
    {
        mainMenu.SetBool("isHidden", false);
        resumeGameButton.SetBool("isHidden", true);
        resumeGamePanel.SetActive(false);

    }

    public void openSettings()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Settings");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void openMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

}
