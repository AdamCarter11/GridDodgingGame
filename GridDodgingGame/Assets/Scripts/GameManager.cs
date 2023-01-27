using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = new GameManager();

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image slot0, slot1, slot2, slot3;
    [SerializeField] Sprite empty, dirTrapRight, dirTrapLeft,
        pushTrapDown, pushTrapLeft, pushTrapRight, pushTrapUp;

    public int[] currentItems;
    //TrapType Enum
    // dirTrapRight = 0,
    // dirTrapLeft = 1,
    // pushTrapDown = 2,
    // pushTrapLeft = 3,
    // pushTrapRight = 4,
    // pushTrapUp = 5,
    
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
        //ShowQueue();
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
        currentItems = PlayerMove.Instance.items.ToArray();
        Image currentSlot = null;

        // Send information to UI
        int i = 0;
        foreach (int item in currentItems)
        {
            if (i == 0) currentSlot = slot0;
            if (i == 1) currentSlot = slot1;
            if (i == 2) currentSlot = slot2;
            if (i == 3) currentSlot = slot3;

            if (currentItems[i] == 0) currentSlot.sprite = empty;
            if (currentItems[i] == 0) currentSlot.sprite = dirTrapRight;
            if (currentItems[i] == 1) currentSlot.sprite = dirTrapLeft;
            if (currentItems[i] == 2) currentSlot.sprite = pushTrapDown;
            if (currentItems[i] == 3) currentSlot.sprite = pushTrapLeft;
            if (currentItems[i] == 4) currentSlot.sprite = pushTrapRight;
            if (currentItems[i] == 5) currentSlot.sprite = pushTrapUp;

            i++;
        }
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
