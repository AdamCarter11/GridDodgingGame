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
    [HideInInspector] public static float time = 1;
    [HideInInspector] public static int playerHealth = 5;
    [HideInInspector] public static int multiplier = 1;
    [SerializeField] private int startingTime;
    private bool pause = false;
    bool diffIncrease = true;
    bool timeThreshText = true;
    [SerializeField] Animator timeTextAnim;

    //visualized timer
    [SerializeField] Image timeIndicator;

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
        timeText.text = "Time: " + (int)time;
        scoreText.text = "Score: " + 0;
        multiplierText.text = "X" + multiplier;
        startingPitch = audioSourcePitch.pitch;

        StartCoroutine(timeScoreIncrease());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        timeText.text = "Time: " + (int)time;
        if(time <= startingTime / 4){
            timeText.color = Color.red;
            timeTextAnim.SetTrigger("timeChange");
            if(timeThreshText){
                timeTextAnim.speed *= 2;
                timeThreshText = false;
            }
            
        }
        else if(time <= startingTime / 2){
            if(!timeThreshText){
                timeTextAnim.speed = 1;
                timeThreshText = true;
            }
            timeText.color = new Color(1, .64f, 0);
            timeTextAnim.SetTrigger("timeChange");
        }
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

        TimerFunc();
    }
    void TimerFunc(){
        if(time > 0){
            time -= Time.deltaTime;
            timeIndicator.enabled = true;
            timeIndicator.fillAmount = time/startingTime;
        }
    }
    public void IncreaseScore(int val)
    {
        score += (val * multiplier);
        if(score >= 500 && diffIncrease){
            Difficulty.instance.spawnCap -= .2f;
            Difficulty.instance.enemySpawnDelayScaling += .05f;
        }
    }

    public void ChangeTime(int val)
    {
        time += val;
    }
    public void ChangeHealth(int val){
        playerHealth += val;
        if(playerHealth <= 0){
            GameOverFunc();
        }
    }

    public void ChangeMultiplier(int value){
        multiplier = value;
    }

    IEnumerator timeScoreIncrease()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            //time--;
            if(time <= 0){
                GameOverFunc();
            }
        }
    }
    private void GameOverFunc(){
        GameManager.Instance.ChangeMultiplier(1);
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
    }
}
