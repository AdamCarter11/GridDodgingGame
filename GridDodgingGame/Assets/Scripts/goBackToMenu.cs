using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goBackToMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    public void returnToMenu(){
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    public void quitGame(){
        Application.Quit();
    }
    public void resume(){
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void restartMainGame(){
        Time.timeScale = 1;
        SceneManager.LoadScene("MainGame");
    }
}
