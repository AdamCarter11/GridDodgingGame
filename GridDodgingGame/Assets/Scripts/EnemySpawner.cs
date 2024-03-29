using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    float spawnDelay;
    float spawnScalingChange;
    float spawnCap;

    private float spawnScaling = 0;

    // Grid spawn borders
    const float START_X = -6.5f;
    const float START_Y = -4.5f;
    const float END_X = 5.5f;
    const float END_Y = 4.5f;
    const float GRID_INTERVAL = 1.0f;

    // Enemy initialization variables
    int dir, pos_x, pos_y;
    Vector3 enemyFacing;
    Vector3 enemyPosition;

    //[SerializeField] EnemyMove enemyMoveScript;

    void Start()
    {
        if(Difficulty.instance.enemySpawnDelay >= 0){
            spawnDelay = Difficulty.instance.enemySpawnDelay;
        }
        if(Difficulty.instance.enemySpawnDelayScaling >= 0){
            spawnScalingChange = Difficulty.instance.enemySpawnDelayScaling;
        }
        spawnCap = Difficulty.instance.spawnCap;
        
        StartCoroutine(spawnEnemy());
        StartCoroutine(ScalingTime());
        //enemyMoveScript.dir = 4;

    }

    
    void Update()
    {
        
    }
    IEnumerator ScalingTime(){
        while(true){
            yield return new WaitForSeconds(4f);
            if(spawnScaling <= spawnDelay - spawnCap){
                    spawnScaling += spawnScalingChange;
            }
        }
    }

    IEnumerator spawnEnemy(){
        while(true){
            float tempSpawnDelay = spawnDelay - spawnScaling;
            print(tempSpawnDelay);
            if(tempSpawnDelay < spawnCap){
                tempSpawnDelay = spawnCap;
            }
            yield return new WaitForSeconds(tempSpawnDelay);

            // Initialize direction and position of enemy
            dir = Random.Range(1,5);
            pos_x = Random.Range(1,10);
            pos_y = Random.Range(1,9);

            //1 = right, 2 = left, 3 = up, 4 = down
            if (dir == 1) {
                // Set direction
                //transform.eulerAngles = new Vector3(0,0,0);
                enemyFacing = new Vector3(0, 0, 180);
                // set position
                enemyPosition = new Vector3(END_X, START_Y + GRID_INTERVAL * pos_y, 0);
            }
            if (dir == 3) {
                // Set direction
                //transform.eulerAngles = new Vector3(0,0,180);
                enemyFacing = new Vector3(0, 0, 0);
                // Set position
                enemyPosition = new Vector3(START_X, START_Y + GRID_INTERVAL * pos_y, 0);
            }
            if (dir == 4) {
                // Set direction
                //transform.eulerAngles = new Vector3(0,0,90);
                enemyFacing = new Vector3(0, 0, 90);
                // Set position
                enemyPosition = new Vector3(START_X + GRID_INTERVAL * pos_x, START_Y, 0);
            }
            if (dir == 2) {
                // Set direction
                //transform.eulerAngles = new Vector3(0,0,270);
                enemyFacing = new Vector3(0, 0, 270);
                // Set position
                enemyPosition = new Vector3(START_X + GRID_INTERVAL * pos_x, END_Y, 0);
            }

            // Construct enemy
            //GameObject currentEnemy = Instantiate(enemy, enemyPosition, Quaternion.identity);
            GameObject currentEnemy = ObjectPooling.instance.GetPooledObject();
            if(currentEnemy != null){
                print("Spawn rat");
                currentEnemy.transform.position = enemyPosition;
                currentEnemy.SetActive(true);
                currentEnemy.transform.eulerAngles = enemyFacing;
                currentEnemy.GetComponent<EnemyMove>().setFacing(dir);
            }

            
        }
    }
}
