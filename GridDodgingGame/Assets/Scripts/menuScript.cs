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

    public void quitGame(){
        Application.Quit();
    }
    
}
