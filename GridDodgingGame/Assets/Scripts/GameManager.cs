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

    // Manage fence traps
    [SerializeField] GameObject[] fenceTraps;
    public int numFenceTraps;
    private bool isFenceActive;
    [SerializeField] LineRenderer pylonLine;
    [SerializeField] EdgeCollider2D pylonCol;
    [SerializeField] GameObject pylonObj;

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
        multiplier = 1;
        multiplierText.text = "X" + multiplier;
        startingPitch = audioSourcePitch.pitch;
        pylonObj.SetActive(false);

        if(Difficulty.instance.whichMode == 1){
            timeIndicator.enabled = true;
            timeText.text = "Time: " + (int)time;
            StartCoroutine(timeScoreIncrease());
        }
        else{
            timeIndicator.enabled = false;
            timeText.text = "Health: " + playerHealth;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        if(Difficulty.instance.whichMode == 1){
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
            else{
                timeText.color = new Color(.16f, 1, 0);
            }
            TimerFunc();
        }
        else{
            timeText.text = "Health: " + playerHealth;
            if(playerHealth == 1){
                timeText.color = Color.red;
                timeTextAnim.SetTrigger("timeChange");
            }
            else{
                timeText.color = new Color(.16f, 1, 0);
            }
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

        // Check if at any given point, 2 fence traps are on the field
        // If there are, enable both and connect a electric fence between
        // them for 10 seconds (ignore others while 2 are active)
        // Then delete traps and reset the check
        if (!isFenceActive)
        {
            if (numFenceTraps >= 2)
            {
                Debug.Log("2 traps detected");
                // Start particle effect for 10 sec
                StartFenceEffect();
                isFenceActive = true;
            }
        }

    }
    public void TriggerPauseMenu()
    {
        if (!pause)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            pause = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            pause = false;
        }
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
        if(playerHealth < startingHealth && score >= scoreHealVal + 200){
            scoreHealVal = score;
            playerHealth++;
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
        //if(PlayerPrefs.HasKey("score")){
            if(Difficulty.instance.diffLevel == 1){
            //easy score
                if (PlayerPrefs.HasKey("easyScore"))
                {
                    if (score > PlayerPrefs.GetInt("easyScore"))
                    {
                        PlayerPrefs.SetInt("easyScore", score);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt("easyScore", score);
                }
                
            }
            if(Difficulty.instance.diffLevel == 2){
                //medium score
                if (PlayerPrefs.HasKey("mediumScore"))
                {
                    if (score > PlayerPrefs.GetInt("mediumScore"))
                    {
                        PlayerPrefs.SetInt("mediumScore", score);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt("mediumScore", score);
                }
                
            }  
            if(Difficulty.instance.diffLevel == 3){
            //hard score
                if (PlayerPrefs.HasKey("hardScore"))
                {
                    if (score > PlayerPrefs.GetInt("hardScore"))
                    {
                        PlayerPrefs.SetInt("hardScore", score);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt("hardScore", score);
                }
                
            }
        //}
        /*
        else{
            if(Difficulty.instance.diffLevel == 1){
                //easy score
                PlayerPrefs.SetInt("easyScore", score);
            }
            if(Difficulty.instance.diffLevel == 2){
                //medium score
                PlayerPrefs.SetInt("mediumScore", score);
            }  
            if(Difficulty.instance.diffLevel == 3){
                //hard score
                PlayerPrefs.SetInt("hardScore", score);
            }
        }
        */
        SceneManager.LoadScene("GameOver");
    }

    private void StartFenceEffect()
    {
        fenceTraps = GameObject.FindGameObjectsWithTag("FenceTrap");

        if (fenceTraps.Length >= 2)
        {
            fenceTraps[0].GetComponent<fenceTrapBehavior>().SwapSprite();
            fenceTraps[1].GetComponent<fenceTrapBehavior>().SwapSprite();
            StartCoroutine(PlayLightningParticleEffect(fenceTraps[0], fenceTraps[1]));
        }
    }

    IEnumerator PlayLightningParticleEffect(GameObject a, GameObject b)
    {
        pylonObj.SetActive(true);
        //LineRenderer CurrentLR = new GameObject().AddComponent<LineRenderer>();
        //CurrentLR.sortingOrder = 3;
        Vector3[] positions = { new Vector3(a.transform.position.x, a.transform.position.y, -3), new Vector3(b.transform.position.x, b.transform.position.y, -3) };
        //CurrentLR.SetPositions(positions);
        pylonLine.SetPositions(positions);

        SetEdgeCol();

        yield return new WaitForSeconds(8f);
        Destroy(a.gameObject);
        Destroy(b.gameObject);
        pylonObj.SetActive(false);
        isFenceActive = false;
        //will need to destroy the line or something here too
    }
    void SetEdgeCol(){
        //For this to work, make sure the lineRenderer's origin is zero, otherwise there will be an offset
        List<Vector2> edges = new List<Vector2>();
        for(int point = 0; point < pylonLine.positionCount; point++){
            Vector3 lineRenderTempPoint = pylonLine.GetPosition(point);
            edges.Add(new Vector2(lineRenderTempPoint.x, lineRenderTempPoint.y));
        }
        pylonCol.SetPoints(edges);
    }
}
