using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour
{
    public static Difficulty instance;
    [HideInInspector] public float enemySpawnDelay;
    [HideInInspector] public float enemySpawnDelayScaling;
    [HideInInspector] public float enemyMoveDelay;
    [HideInInspector] public float spawnCap; 
    [HideInInspector] public float trapSpawnDelay;

    [HideInInspector] public bool screenshake;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject); 
    }

    public void easyMode(){
        enemySpawnDelay = 2f;
        enemySpawnDelayScaling = .01f;
        enemyMoveDelay = 2f;
        spawnCap = .8f;
        trapSpawnDelay = 3f;
        SceneManager.LoadScene("MainGame");
    }
    public void mediumMode(){
        enemySpawnDelay = 1.5f;
        enemySpawnDelayScaling = .05f;
        enemyMoveDelay = 1.5f;
        spawnCap = .7f;
        trapSpawnDelay = 2f;
        SceneManager.LoadScene("MainGame");
    }
    public void hardMode(){
        enemySpawnDelay = 1f;
        enemySpawnDelayScaling = .1f;
        enemyMoveDelay = 1f;
        spawnCap = .5f;
        trapSpawnDelay = 1f;
        SceneManager.LoadScene("MainGame");
    }
}
