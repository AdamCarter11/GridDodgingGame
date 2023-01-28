using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = new GameManager();
    
    [SerializeField] TextMeshProUGUI scoreText, timeText;
    [HideInInspector] public static int score = 0;
    [HideInInspector] public static int time = 1;
    [SerializeField] private int startingTime;
    private GameManager() {

    }

    public static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = new GameManager();
            }

            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        time = startingTime;
        timeText.text = "Time: " + time;
        scoreText.text = "Score: " + 0;

        StartCoroutine(timeScoreIncrease());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        timeText.text = "Time: " + time;
    }

    public void IncreaseScore(int val)
    {
        score += val;
    }

    public void ChangeTime(int val)
    {
        time += val;
    }

    IEnumerator timeScoreIncrease()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            time--;
            if(time <= 0){
                print("Gameover");
                PlayerPrefs.SetInt("tempScore", score);
                if(PlayerPrefs.HasKey("score")){
                    if(score > PlayerPrefs.GetInt("score")){
                        PlayerPrefs.SetInt("score", score);
                    }
                }
                else{
                    PlayerPrefs.SetInt("score", score);
                }
                SceneManager.LoadScene("GameOver");
                //gameover logic
            }
        }
    }
}
