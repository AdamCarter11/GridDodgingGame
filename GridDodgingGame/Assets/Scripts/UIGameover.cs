using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameover : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText, highScoreText;
    [SerializeField] Sprite goldRat;
    [SerializeField] Image ratImage1, ratImage2;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "your score: " + PlayerPrefs.GetInt("tempScore");
        if(Difficulty.instance.diffLevel == 1){
            highScoreText.text = "highscore: " + PlayerPrefs.GetInt("easyScore");
        }
        if(Difficulty.instance.diffLevel == 2){
            highScoreText.text = "highscore: " + PlayerPrefs.GetInt("mediumScore");
        }
        if(Difficulty.instance.diffLevel == 3){
            highScoreText.text = "highscore: " + PlayerPrefs.GetInt("hardScore");
        }
        
        if(PlayerPrefs.GetInt("tempScore") == PlayerPrefs.GetInt("score")){
            ratImage1.sprite = goldRat;
            ratImage2.sprite = goldRat;
        }
    }
    public void startGame(){
        SceneManager.LoadScene("MainGame");
    }
    public void goToMenu(){
        SceneManager.LoadScene("Menu");
    }
    public void quitGame(){
        Application.Quit();
    }
}
