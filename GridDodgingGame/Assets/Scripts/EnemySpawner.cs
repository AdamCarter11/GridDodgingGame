using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] float spawnDelay;
    private float spawnScaling = 0;
    //[SerializeField] EnemyMove enemyMoveScript;

    void Start()
    {
        StartCoroutine(spawnEnemy());
        //enemyMoveScript.dir = 4;
    }

    
    void Update()
    {
        
    }

    IEnumerator spawnEnemy(){
        while(true){
            yield return new WaitForSeconds(spawnDelay - spawnScaling);
            Instantiate(enemy, new Vector3(5.5f, 1.5f, 0), Quaternion.identity);
            if(spawnScaling <= 2){
                spawnScaling += .1f;
            }
        }
    }
}
