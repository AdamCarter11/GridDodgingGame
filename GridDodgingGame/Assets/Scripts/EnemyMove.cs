using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float enemySpeed;

    float enemyMoveDelayRat;

    [SerializeField] Transform movePoint;
    [SerializeField] GameObject gold;
    [SerializeField] GameObject extraGold;
    [SerializeField] Sprite enchantedSprite;
    [SerializeField] Sprite normalRat;
    [SerializeField] Sprite berzerkSprite;
    private Camera cam;

    //float ratMoveDelay = 2f;
    bool canMove = false;
    bool canBeDamaged = false;
    bool frozen = false;
    bool isBerserk = false;
    bool movingDelayCo = true;
    float startMoveDelay;
    bool launched = false;
    int dir;
    [SerializeField] private int health;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        movePoint.transform.position = transform.position;
        movePoint.parent = null;
        if(Difficulty.instance.enemyMoveDelay >= 0){
            enemyMoveDelayRat = Difficulty.instance.enemyMoveDelay;
            //print("mode select");
        }
        else{
            enemyMoveDelayRat = 2f;
        }
        startMoveDelay = enemyMoveDelayRat;
        StartCoroutine(triggerMove());
    }

    private void OnEnable() {
        canBeDamaged = false;
        frozen = false;
        isBerserk = false;
        launched = false;
        this.GetComponent<SpriteRenderer>().sprite = normalRat;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        movePoint.transform.position = transform.position;
        if(Difficulty.instance.enemyMoveDelay >= 0){
            enemyMoveDelayRat = Difficulty.instance.enemyMoveDelay;
            //print("mode select");
        }
        else{
            enemyMoveDelayRat = 2f;
        }
        startMoveDelay = enemyMoveDelayRat;
        StartCoroutine(triggerMove());
    }

    
    void Update()
    {
        //moves player
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, enemySpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f){
            if (!isBerserk)
            {
                //right
                if (canMove && dir == 1)
                {
                    movePoint.position += new Vector3(-1f, 0f, movePoint.position.z);
                    canMove = false;
                }
                //left
                if (canMove && dir == 3)
                {
                    movePoint.position += new Vector3(1f, 0f, movePoint.position.z);
                    canMove = false;
                }
                //up
                if (canMove && dir == 4)
                {
                    movePoint.position += new Vector3(0f, 1f, movePoint.position.z);
                    canMove = false;
                }
                //down
                if (canMove && dir == 2)
                {
                    movePoint.position += new Vector3(0f, -1f, movePoint.position.z);
                    canMove = false;
                }
            }
            else
            {
                //print("AI calc player pos at " + PlayerMove.Instance.GetPlayerPos());
                // AI movement towards player
                //Vector3 currentPlayerPos = PlayerMove.Instance.GetPlayerPos();
                Vector3 currentPlayerPos = GameObject.Find("Player").GetComponent<PlayerMove>().GetPlayerPos();
                float xDiff, yDiff;
                xDiff = Mathf.Abs(currentPlayerPos.x - transform.position.x);
                yDiff = Mathf.Abs(currentPlayerPos.y - transform.position.y);
                if (xDiff > yDiff)
                {
                    if (canMove && currentPlayerPos.x - transform.position.x > 0)
                    {
                        // Move left
                        movePoint.position += new Vector3(1f, 0f, movePoint.position.z);
                        canMove = false;
                    }
                    else if(canMove)
                    {
                        // Move right
                        movePoint.position += new Vector3(-1f, 0f, movePoint.position.z);
                        canMove = false;
                    }
                }
                else
                {
                    if (canMove && currentPlayerPos.y - transform.position.y > 0)
                    {
                        // Move up
                        movePoint.position += new Vector3(0f, 1f, movePoint.position.z);
                        canMove = false;
                    }
                    else if(canMove)
                    {
                        // Move down
                        movePoint.position += new Vector3(0f, -1f, movePoint.position.z);
                        canMove = false;
                    }
                }
            }
        }
    }

    // Set facing of enemy
    public void setFacing(int facing)
    {
        dir = facing;
    }

    IEnumerator triggerMove(){
        while(true){
            yield return new WaitForSeconds(enemyMoveDelayRat);
            if(movingDelayCo){
                canMove = true;
            }
            //moveDelay = startMoveDelay;
        }
    }
    IEnumerator movingDelay(){
        movingDelayCo = false;
        yield return new WaitForSeconds(2f);
        movingDelayCo = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("LaunchTrap")){
            if(!isBerserk){
                this.GetComponent<SpriteRenderer>().color = Color.green;
                enemyMoveDelayRat = .1f;
                launched = true;
            }
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("hole")){
            StartCoroutine(movingDelay());
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("Wall") && canBeDamaged){
            //Destroy(movePoint.gameObject);
            //Destroy(gameObject); 
            canBeDamaged = false;
            gameObject.SetActive(false);
        }
        if(other.gameObject.CompareTag("enemy")){
            //disabled screenshake on enemies for now
            //cam.GetComponent<ScreenShake>().TriggerShake();
            if(PlayerPrefs.GetInt("ScreenShake") > 0){
                cam.GetComponent<ScreenShake>().TriggerShake();
            }
            //Destroy(other.gameObject);
            if(!launched){
                if(!isBerserk){
                    //Destroy(this.gameObject);
                    //Destroy(movePoint.gameObject);
                    if(this.GetComponent<SpriteRenderer>().sprite == enchantedSprite){
                        Instantiate(extraGold, transform.position, Quaternion.identity);
                    }else{
                        Instantiate(gold, transform.position, Quaternion.identity);
                    }
                    gameObject.SetActive(false);
                }
                else if(isBerserk){
                    gameObject.SetActive(false);
                }
                else{
                    health--;
                    if(health <= 0){
                        //Destroy(this.gameObject);
                        //Destroy(movePoint.gameObject);
                        if(this.GetComponent<SpriteRenderer>().sprite == enchantedSprite){
                            Instantiate(extraGold, transform.position, Quaternion.identity);
                        }else{
                            Instantiate(gold, transform.position, Quaternion.identity);
                        }
                        gameObject.SetActive(false);
                    }
                }
                //object pooling code
                
            }
            
        }
        if(other.gameObject.CompareTag("dirTrap")){
            transform.Rotate(new Vector3(0.0f,0.0f,90.0f), Space.Self);
            print(dir);
            dir++;
            //if(dir == 2){ dir = 1; }
            //if(dir == 4){ dir = 3; }
            if(dir > 4){
                dir %= 4;
            }
            print(dir);
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("dirTrapRight")){
            transform.Rotate(new Vector3(0.0f,0.0f,-90.0f), Space.Self);
            print(dir);
            dir--;
            if(dir == 2){ dir = 3; }
            if(dir == 4){ dir = 1; }
            if(dir <= 0){
                dir = 4;
            }
            print(dir);
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("pushTrapUp")){
            movePoint.position += new Vector3(0f, 1f, movePoint.position.z);
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("pushTrapDown")){
            movePoint.position += new Vector3(0f, -1f, movePoint.position.z);
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("pushTrapRight")){
            movePoint.position += new Vector3(1f, 0f, movePoint.position.z);
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("pushTrapLeft")){
            movePoint.position += new Vector3(-1f, 0f, movePoint.position.z);
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("Gold")){
            if(!launched){
                Destroy(other.gameObject);
                this.GetComponent<SpriteRenderer>().sprite = enchantedSprite;
            }
        }
        if (other.gameObject.CompareTag("betterGold"))
        {
            if(!launched){
                isBerserk = true;
                this.GetComponent<SpriteRenderer>().color = Color.red;
                Destroy(other.gameObject);
            }
            //this.GetComponent<SpriteRenderer>().sprite = berzerkSprite;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Wall")){
            canBeDamaged = true;
            //print("left wall");
        }
    }
}
