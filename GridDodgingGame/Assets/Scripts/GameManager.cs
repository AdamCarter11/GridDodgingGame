using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = new GameManager();

    [SerializeField] TextMeshProUGUI scoreText, timeText;
    [HideInInspector] public static int score = 0;
    [HideInInspector] public static int time;
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
        ShowQueue();
    }

    public void IncreaseScore(int val)
    {
        score += val;
    }

    public void ChangeTime(int val)
    {
        time += val;
    }

    public void ShowQueue()
    {
        // Access player items
        //PlayerMove.items;

        // Send information to UI
    }

    IEnumerator timeScoreIncrease()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            time--;
            if(time <= 0){
                print("Gameover");
                //gameover logic
            }
        }
    }
}

public enum GameStates {
    MainMenu,
    Game,
    GameOver,
    Paused
}
