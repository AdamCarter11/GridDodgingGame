using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject difficultyPanel;
    [SerializeField] GameObject highScorePanel;
    [SerializeField] GameObject creditsPanel;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("score")){
            scoreText.text = "High Score: " + PlayerPrefs.GetInt("score");
        }
        else{
            scoreText.text = "High Score: YOU?";
        }
        if(!PlayerPrefs.HasKey("volume")){
            PlayerPrefs.SetFloat("volume", 1);
            AudioListener.volume = 1;
        }
        else{
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
        }
    }

    public void startGame(){
        difficultyPanel.SetActive(true);
    }

    public void returnFromDifficulty()
    {
        difficultyPanel.SetActive(false);
    }
    public void enterTutorial()
    {
        tutorialPanel.SetActive(true);
    }
    public void returnFromTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    public void enterHighScore()
    {
        highScorePanel.SetActive(true);
    }
    public void enterCredits()
    {
        creditsPanel.SetActive(true);
    }
    public void returnFromHighScore()
    {
        highScorePanel.SetActive(false);
    }
    public void returnFromCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void quitGame(){
        Application.Quit();
    }
    
}
