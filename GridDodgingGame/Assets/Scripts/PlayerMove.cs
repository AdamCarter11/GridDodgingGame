using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class PlayerMove : MonoBehaviour
{
    private static PlayerMove _instance = new PlayerMove();

    [SerializeField] float moveSpeed;
    [SerializeField] Transform movePoint;
    [SerializeField] LayerMask whatStopsMovement;
    [SerializeField] GameObject[] traps;
    //[SerializeField] TextMeshProUGUI scoreText;
    //[HideInInspector] public static int score = 0;
    bool canDig = false;
    GameObject currDiggingTile;
    float[] possibleXVals = new float[11];
    float[] possibleYVals = new float[8];

    public Queue<int> items = new Queue<int>();
    //TrapType Enum
    // dirTrapRight = 0,
    // dirTrapLeft = 1,
    // pushTrapDown = 2,
    // pushTrapLeft = 3,
    // pushTrapRight = 4,
    // pushTrapUp = 5,

    [SerializeField] int health;

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

    void Start()
    {

        //removes the movepoint from the player (keeps things organized)
        movePoint.parent = null;
        //scoreText.text = "Score: " + 0;
        setupTiles();
        //StartCoroutine(timeScoreIncrease());
        
        // Load all the items as -1
        for (int i = 0; i < 4; i++)
        {
            items.Enqueue(-1);
        }
    }

    void Update()
    {
        DiggingLogic();
        placeTrapLogic();

        //moves player
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f){
            //left-right input
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f){
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, movePoint.position.z), .2f, whatStopsMovement)){
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, movePoint.position.z);
                }
            }
            //top-down input
            if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f){
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), movePoint.position.z), .2f, whatStopsMovement)){
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), movePoint.position.z);
                }
            }
        }

        //scoreText.text = "Score: " + score;
    }

    //IEnumerator timeScoreIncrease(){
    //    while(true){
    //        yield return new WaitForSeconds(1f);
    //        score++;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("DigginTile")){
            canDig = true;
            currDiggingTile = other.gameObject;
        }
        if(other.gameObject.CompareTag("enemy")){
            Destroy(other.gameObject);
            health--;
            if(health <= 0){
                //gameover
                print("gameover!");
            }
        }
        if(other.gameObject.CompareTag("Gold")){
            Destroy(other.gameObject);
            //increase score
            GameManager.Instance.IncreaseScore(10);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("DigginTile")){
            canDig = false;
        }
    }

    void DiggingLogic(){
        if(Input.GetKeyDown(KeyCode.Space) && canDig){
            print("dug tile");
            currDiggingTile.transform.position = new Vector3(possibleXVals[Random.Range(0,possibleXVals.Length)], possibleYVals[Random.Range(0,possibleYVals.Length)], currDiggingTile.transform.position.z);
            if(items.Count < 2){
                items.Enqueue(Random.Range(0,6));
            }
            else{
                items.Dequeue();
                items.Enqueue(Random.Range(0,6));
            }
        }

        GameManager.Instance.ShowQueue();
    }

    void placeTrapLogic(){
        if(Input.GetKeyDown(KeyCode.F)){
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
