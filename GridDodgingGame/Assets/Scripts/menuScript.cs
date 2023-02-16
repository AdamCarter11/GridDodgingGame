using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject difficultyPanel;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("score")){
            scoreText.text = "Highscore: " + PlayerPrefs.GetInt("score");
        }
        else{
            scoreText.text = "Highscore: YOU?";
        }
    }

    public void startGame(){
        difficultyPanel.SetActive(true);
    }

    public void quitGame(){
        Application.Quit();
    }
    
}
