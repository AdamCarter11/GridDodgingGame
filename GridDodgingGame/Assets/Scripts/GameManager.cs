using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = new GameManager();
    
    [SerializeField] TextMeshProUGUI scoreText, timeText, multiplierText;
    [SerializeField] AudioSource audioSourcePitch;
    float startingPitch;
    [SerializeField] GameObject pauseMenu;
    [HideInInspector] public static int score = 0;
    [HideInInspector] public static int time = 1;
    [HideInInspector] public static int multiplier = 1;
    [SerializeField] private int startingTime;
    private bool pause = false;
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
        multiplierText.text = "X" + multiplier;
        startingPitch = audioSourcePitch.pitch;

        StartCoroutine(timeScoreIncrease());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        timeText.text = "Time: " + time;
        multiplierText.text = "X" + multiplier;
        if(multiplier == 1){
            multiplierText.faceColor = Color.white;
        }
        else if(multiplier == 2){
            multiplierText.faceColor = Color.green;
        }
        else if(multiplier > 2){
            multiplierText.faceColor = Color.yellow;
        }

        //Pause logic
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!pause){
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                pause = true;
            }
            else{
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                pause = false;
            }
        }
    }

    public void IncreaseScore(int val)
    {
        score += (val * multiplier);
    }

    public void ChangeTime(int val)
    {
        time += val;
    }

    public void ChangeMultiplier(int value){
        multiplier = value;
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
