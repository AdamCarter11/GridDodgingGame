using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float enemySpeed;
    [SerializeField] float moveDelay;
    [SerializeField] Transform movePoint;
    bool canMove = false;
    bool canBeDamaged = false;
    int dir;

    void Start()
    {
        movePoint.parent = null;
        StartCoroutine(triggerMove());
    }

    
    void Update()
    {
        //moves player
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, enemySpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f){
            //right
            if(canMove && dir == 1){
                movePoint.position += new Vector3(-1f, 0f, movePoint.position.z);
                canMove = false;
            }
            //left
            if(canMove && dir == 3){
                movePoint.position += new Vector3(1f, 0f, movePoint.position.z);
                canMove = false;
            }
            //up
            if(canMove && dir == 4){
                movePoint.position += new Vector3(0f, 1f, movePoint.position.z);
                canMove = false;
            }
            //down
            if(canMove && dir == 2){
                movePoint.position += new Vector3(0f, -1f, movePoint.position.z);
                canMove = false;
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
            yield return new WaitForSeconds(moveDelay);
            canMove = true;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Wall") && canBeDamaged){
            Destroy(movePoint.gameObject);
            Destroy(gameObject); 
        }
        if(other.gameObject.CompareTag("enemy")){
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            Destroy(movePoint.gameObject);
            PlayerMove.score += 10;
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
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Wall")){
            canBeDamaged = true;
        }
    }
}
