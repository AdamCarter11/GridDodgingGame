using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moleSpawner : MonoBehaviour
{
    const float START_X = -5.5f;
    const float START_Y = -2.5f;
    const float END_X = 2.5f;
    const float END_Y = 2.5f;
    const float GRID_INTERVAL = 1.0f;
    float spawnTime;
    Vector3 spawnPos;
    int pos_x, pos_y;
    [SerializeField] int moleSpawnDelay = 2;
    [SerializeField] int moleLifetime;
    [SerializeField] GameObject moleHoleObj, moleObj;

    void Start()
    {
        spawnTime = Random.Range(2, 5);
        StartCoroutine(SpawnMoleHole());
    }

    IEnumerator SpawnMoleHole(){
        while(true){
            yield return new WaitForSeconds(spawnTime);
            spawnTime = Random.Range(2, 5);
            pos_x = Random.Range(1,9);
            pos_y = Random.Range(1,8);
            spawnPos = new Vector3(START_X + GRID_INTERVAL * pos_x, START_Y + GRID_INTERVAL * pos_y, 0);
            GameObject tempMoleHole = Instantiate(moleHoleObj, spawnPos, Quaternion.identity);
            Destroy(tempMoleHole, moleSpawnDelay);
            StartCoroutine(SpawnMole());
        }
    }
    IEnumerator SpawnMole(){
        yield return new WaitForSeconds(moleSpawnDelay);
        GameObject tempMole = Instantiate(moleObj, spawnPos, Quaternion.identity);
        Destroy(tempMole, moleLifetime);
    }
}
