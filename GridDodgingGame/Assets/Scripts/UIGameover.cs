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
        highScoreText.text = "highscore: " + PlayerPrefs.GetInt("score");
        if(PlayerPrefs.GetInt("tempScore") == PlayerPrefs.GetInt("score")){
            ratImage1.sprite = goldRat;
            ratImage2.sprite = goldRat;
        }
    }
    public void startGame(){
        SceneManager.LoadScene("MainGame");
    }

}
