using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] TextMeshProUGUI scoreText, timeText, multiplierText;
    [SerializeField] AudioSource audioSourcePitch;
    float startingPitch;
    [SerializeField] GameObject pauseMenu;
    [HideInInspector] public static int score = 0;
    [HideInInspector] public static float time = 1;
    [SerializeField] int startingHealth;
    [HideInInspector] public static int playerHealth;
    [HideInInspector] public static int multiplier = 1;
    [SerializeField] private int startingTime;
    private bool pause = false;
    bool diffIncrease = true;
    bool timeThreshText = true;
    [SerializeField] Animator timeTextAnim;
    int scoreHealVal;

    // Grid spawn borders (from EnemySpawner)
    public const float START_X = -7.5f;
    public const float START_Y = -4.5f;
    public const float END_X = 4.5f;
    public const float END_Y = 4.5f;
    public const float GRID_INTERVAL = 1.0f;

    //visualized timer
    [SerializeField] Image timeIndicator;

    private void Awake() 
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        if (Instance == null)
        {
            Instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = startingHealth;
        score = 0;
        time = startingTime;
        scoreText.text = "Score: " + 0;
        multiplierText.text = "X" + multiplier;
        startingPitch = audioSourcePitch.pitch;

        if(Difficulty.instance.whichMode == 1){
            timeText.text = "Time: " + (int)time;
            StartCoroutine(timeScoreIncrease());
        }
        else{
            timeText.text = "Health: " + playerHealth;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        if(Difficulty.instance.whichMode == 1){
            timeText.text = "Time: " + (int)time;
        }
        else{
            timeText.text = "Health: " + playerHealth;
        }
        
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
        //set the increase health thresh here (200)
        if(playerHealth < 5 && score >= scoreHealVal + 200){
            scoreHealVal = score;
            playerHealth++;
        }
        if(score >= 500 && diffIncrease){
            Difficulty.instance.spawnCap -= .2f;
            Difficulty.instance.enemySpawnDelayScaling += .05f;
        }
    }

    public void ChangeTime(int val)
    {
        if(Difficulty.instance.whichMode == 1){
            time += val;
        }
    }
    public void ChangeHealth(int val){
        if(Difficulty.instance.whichMode == 2){
            playerHealth += val;
            if(playerHealth <= 0){
                GameOverFunc();
            }
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
