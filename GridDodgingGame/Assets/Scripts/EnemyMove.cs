using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float enemySpeed;
    [SerializeField] float moveDelay;
    [SerializeField] Transform movePoint;
    bool canMove = false;
    int dir;
    bool canBeDamaged = false;

    void Start()
    {
        movePoint.parent = null;
        dir = Random.Range(1,5);
        //dir = 2;
        //1 = right, 2 = left, 3 = up, 4 = down
        if(dir == 1){
            transform.eulerAngles = new Vector3(0,0,0);
        }
        if(dir == 2){
            transform.eulerAngles = new Vector3(0,0,180);
        }
        if(dir == 3){
            transform.eulerAngles = new Vector3(0,0,90);
        }
        if(dir == 4){
            transform.eulerAngles = new Vector3(0,0,270);
        }
        StartCoroutine(triggerMove());
    }

    
    void Update()
    {
        //moves player
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, enemySpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f){
            //right
            if(canMove && dir == 1){
                movePoint.position += new Vector3(1f, 0f, movePoint.position.z);
                canMove = false;
            }
            //left
            if(canMove && dir == 2){
                movePoint.position += new Vector3(-1f, 0f, movePoint.position.z);
                canMove = false;
            }
            //up
            if(canMove && dir == 3){
                movePoint.position += new Vector3(0f, 1f, movePoint.position.z);
                canMove = false;
            }
            //down
            if(canMove && dir == 4){
                movePoint.position += new Vector3(0f, -1f, movePoint.position.z);
                canMove = false;
            }
        }
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
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Wall")){
            canBeDamaged = true;
        }
    }
}
