using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject difficultyPanel;
    [SerializeField] GameObject highScorePanel;
    [SerializeField] GameObject creditsPanel;

    [SerializeField] Sprite tutorial1, tutorial2, tutorial3, tutorial4;
    enum TutorialState
    {
        t0 = -1,
        t1 = 1,
        t2 = 2,
        t3 = 3,
        t4 = 4
    };
    TutorialState currentTutorialUIState;

    // Start is called before the first frame update
    void Start()
    {
        currentTutorialUIState = TutorialState.t0;

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

    public void NextTutorialUI()
    {
        currentTutorialUIState++;
        // max state
        if ((int)currentTutorialUIState == 5)
        {
            currentTutorialUIState = TutorialState.t1;
        }
        tutorialPanel.GetComponent<Image>().sprite = GetSpriteBasedOnState(currentTutorialUIState);
    }
    public void PreviousTutorialUI()
    {
        currentTutorialUIState--;
        // max state
        if ((int)currentTutorialUIState == 0)
        {
            currentTutorialUIState = TutorialState.t4;
        }
        tutorialPanel.GetComponent<Image>().sprite = GetSpriteBasedOnState(currentTutorialUIState);
    }

    private Sprite GetSpriteBasedOnState(TutorialState val)
    {
        switch (val)
        {
            case TutorialState.t1:
                return tutorial1;
                break;
            case TutorialState.t2:
                return tutorial2;
                break;
            case TutorialState.t3:
                return tutorial3;
                break;
            case TutorialState.t4:
                return tutorial4;
                break;
            default:
                return null;
                Debug.LogError("Tutorial UI issues");
                break;
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
        tutorialPanel.GetComponent<Image>().sprite = tutorial1;
        currentTutorialUIState = TutorialState.t1;
    }
    public void returnFromTutorial()
    {
        currentTutorialUIState = TutorialState.t0;
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

    public void ClearSaves(){
        PlayerPrefs.DeleteAll();
    }
    
}
