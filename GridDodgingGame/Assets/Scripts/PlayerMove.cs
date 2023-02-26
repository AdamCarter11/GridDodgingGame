using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class PlayerMove : MonoBehaviour
{
    //private static PlayerMove _instance = new PlayerMove();

    [SerializeField] float moveSpeed;
    [SerializeField] Transform movePoint;
    [SerializeField] LayerMask whatStopsMovement;
    [SerializeField] GameObject[] traps;
    //[SerializeField] TextMeshProUGUI scoreText;
    //[HideInInspector] public static int score = 0;
    bool canDig = false, movePause = false;
    GameObject currDiggingTile;
    float[] possibleXVals = new float[11];
    float[] possibleYVals = new float[8];

    public Queue<int> items = new Queue<int>();

    int multiplierStreak = 0;
    int currMultiplier = 1;

    [SerializeField] GameObject holeTile;
    [SerializeField] Canvas canvas;
    [SerializeField] Image currentImage;
    [SerializeField] Image[] imageSlots;
    [SerializeField] Sprite empty, dirTrapRight, dirTrapLeft,
        pushTrapDown, pushTrapLeft, pushTrapRight, pushTrapUp;
    //TrapType Enum
    // dirTrapRight = 0,
    // dirTrapLeft = 1,
    // pushTrapDown = 2,
    // pushTrapLeft = 3,
    // pushTrapRight = 4,
    // pushTrapUp = 5,

    private bool dir = true;
    bool movingDelayCo = true;
    bool canSlow = false;

    [SerializeField] int health;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sfx;
    private Camera cam;
    [SerializeField] private ParticleSystem particleMinus;
    Animator playerAnimator;

    /*
    public static PlayerMove Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerMove();
            }

            return _instance;
        }
    }
    */
    
    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //canvas = GetComponent<Canvas>();
        imageSlots = canvas.GetComponentsInChildren<Image>();

        //removes the movepoint from the player (keeps things organized)
        movePoint.parent = null;
        //scoreText.text = "Score: " + 0;
        setupTiles();
        if(!PlayerPrefs.HasKey("ScreenShake")){
            PlayerPrefs.SetInt("ScreenShake" , 1);
        }
        //StartCoroutine(timeScoreIncrease());
    }

    void Update()
    {
        DiggingLogic();

        ShowQueue();

        //moves player
        moveLogic();

    }
    void moveLogic(){
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if(dir == false && movePoint.transform.position.x > transform.position.x){
            Flip();
        }
        if(dir && movePoint.transform.position.x < transform.position.x){
            Flip();
        }
        if(Vector3.Distance(transform.position, movePoint.position) <= .05f){
            if(!movePause){
            //left-right input
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f && movingDelayCo){
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, movePoint.position.z), .2f, whatStopsMovement)){
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, movePoint.position.z);
                    StartCoroutine(onMovePause());
                }
            }
            //top-down input
            if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f && movingDelayCo){
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), movePoint.position.z), .2f, whatStopsMovement)){
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), movePoint.position.z);
                    StartCoroutine(onMovePause());
                }
            }
            }
        }
    }
    IEnumerator movingDelay(){
        movingDelayCo = false;
        yield return new WaitForSeconds(1f);
        movingDelayCo = true;
    }
    IEnumerator onMovePause(){
        //used to add a slight pause between player movement (.4 and higher is too long)
        movePause = true;
        yield return new WaitForSeconds(.2f);
        movePause = false;
    }
    void Flip(){
        dir = !dir;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    //IEnumerator timeScoreIncrease(){
    //    while(true){
    //        yield return new WaitForSeconds(1f);
    //        score++;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("hole") && canSlow){
            Destroy(other.gameObject);
            StartCoroutine(movingDelay());
        }
        if(other.gameObject.CompareTag("DigginTile")){
            canDig = true;
            currDiggingTile = other.gameObject;
        }
        if(other.gameObject.CompareTag("enemy")){
            multiplierStreak = 0;
            GameManager.Instance.ChangeMultiplier(1);
            currMultiplier = 1;
            if(PlayerPrefs.GetInt("ScreenShake") > 0){
                cam.GetComponent<ScreenShake>().TriggerShake();
            }
            Instantiate(particleMinus, transform.position, Quaternion.identity);
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            GameManager.Instance.ChangeTime(-5);
            health--;
            if(health <= 0){
                //gameover
                print("gameover!");
            }
        }
        if(other.gameObject.CompareTag("Gold")){
            audioSource.PlayOneShot(sfx[0], .7f);
            Destroy(other.gameObject);
            multiplierStreakFunc();
            //increase score
            GameManager.Instance.IncreaseScore(10);
            if(currMultiplier == 4){
                GameManager.Instance.ChangeTime(1);
            }
        }
        if(other.gameObject.CompareTag("betterGold")){
            audioSource.PlayOneShot(sfx[0], .7f);
            Destroy(other.gameObject);
            multiplierStreakFunc();
            //increase score
            GameManager.Instance.IncreaseScore(20);
            GameManager.Instance.ChangeTime(1*currMultiplier);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("DigginTile")){
            canDig = false;
        }
        if(other.gameObject.CompareTag("hole") && !canSlow){
            canSlow = true;
        }
    }

    void multiplierStreakFunc(){
        multiplierStreak++;
        if(multiplierStreak > 5 && currMultiplier == 1){
            GameManager.Instance.ChangeMultiplier(2);
            currMultiplier = 2;
        }
        else if(multiplierStreak > 15 && currMultiplier == 2){
            GameManager.Instance.ChangeMultiplier(4);
            currMultiplier = 4;
        }
    }
    void DiggingLogic(){
        if(Input.GetKeyDown(KeyCode.Space)){
            canSlow = false;
            if(canDig){
                playerAnimator.SetTrigger("dig");
                GameObject spawnedHoleTile = Instantiate(holeTile, movePoint.transform.position, Quaternion.identity);
                Destroy(spawnedHoleTile, 3f);
                audioSource.PlayOneShot(sfx[1], .7f);
                print("dug tile");
                currDiggingTile.SetActive(false);
                StartCoroutine(spawnDigTile());
                if(items.Count < 4){
                    items.Enqueue(Random.Range(0,6));
                }
                else{
                    items.Dequeue();
                    items.Enqueue(Random.Range(0,6));
                }
            }
            else{
                if(items.Count > 0){
                    int trap = items.Dequeue();
                    //intantiate trap where we are standing
                    Instantiate(traps[trap], movePoint.transform.position, Quaternion.identity);
                }
                else{
                    print("no items");
                }
            }
        }
        
    }

    public Vector3 GetPlayerPos()
    {
        return transform.position;
    }

    IEnumerator spawnDigTile(){
        yield return new WaitForSeconds(3f);
        currDiggingTile.transform.position = new Vector3(possibleXVals[Random.Range(0,possibleXVals.Length)], possibleYVals[Random.Range(0,possibleYVals.Length)], currDiggingTile.transform.position.z);
        currDiggingTile.SetActive(true);
    }

    public void ShowQueue()
    {
        // Send information to UI
        int i;
        for (i = 0; i < items.ToArray().Length; ++i)
        {
            if (i == 0) currentImage = GameObject.Find("queue0").GetComponent<Image>();
            else if (i == 1) currentImage = GameObject.Find("queue1").GetComponent<Image>();
            else if (i == 2) currentImage = GameObject.Find("queue2").GetComponent<Image>();
            else if (i == 3) currentImage = GameObject.Find("queue3").GetComponent<Image>();

            if (items.ToArray()[i] == 0) currentImage.sprite = dirTrapLeft;
            else if (items.ToArray()[i] == 1) currentImage.sprite = dirTrapRight;
            else if (items.ToArray()[i] == 2) currentImage.sprite = pushTrapDown;
            else if (items.ToArray()[i] == 3) currentImage.sprite = pushTrapLeft;
            else if (items.ToArray()[i] == 4) currentImage.sprite = pushTrapRight;
            else if (items.ToArray()[i] == 5) currentImage.sprite = pushTrapUp;
            else currentImage.sprite = empty;
        }
        while (i < 4)
        {
            currentImage = GameObject.Find($"queue{i}").GetComponent<Image>();
            currentImage.sprite = empty;
            i++;
        }
    }

    void setupTiles(){
        float xStart = -6.5f;
        for(int i = 0; i < possibleXVals.Length; i++){
            possibleXVals[i] = xStart;
            xStart += 1.0f;
        }
        float yStart = -3.5f;
        for(int i = 0; i < possibleYVals.Length; i++){
            possibleYVals[i] = yStart;
            yStart += 1.0f;
        }
    }
}
