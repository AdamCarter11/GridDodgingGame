using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = new GameManager();

    [SerializeField] TextMeshProUGUI scoreText;
    [HideInInspector] public static int score = 0;

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
        scoreText.text = "Score: " + 0;
        StartCoroutine(timeScoreIncrease());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        ShowQueue();
    }

    public void IncreaseScore(int val)
    {
        score += val;
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
            score++;
        }
    }
}

public enum GameStates {
    MainMenu,
    Game,
    GameOver,
    Paused
}
