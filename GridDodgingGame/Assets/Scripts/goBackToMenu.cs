using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goBackToMenu : MonoBehaviour
{
    public void returnToMenu(){
        SceneManager.LoadScene("Menu");
    }
}
