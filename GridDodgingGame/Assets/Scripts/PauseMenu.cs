using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle shakeToggle;
    [SerializeField] TextMeshProUGUI highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("volume")){
            PlayerPrefs.SetFloat("volume", 1);
            LoadVolume();
        }
        else{
            LoadVolume();
        }
        if(!PlayerPrefs.HasKey("ScreenShake")){
            PlayerPrefs.SetInt("ScreenShake" , 1);
        }
        else{
            if(PlayerPrefs.GetInt("ScreenShake") == 1){
                shakeToggle.isOn = true;
            }
            else{
                shakeToggle.isOn = false;
            }
        }
        shakeToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(shakeToggle);});
        if(Difficulty.instance.diffLevel == 1){
            if(PlayerPrefs.HasKey("easyScore"))
                highScoreText.text = "Current highscore: " + PlayerPrefs.GetInt("easyScore");
            else
                highScoreText.text = "YOU?";
        }
        else if(Difficulty.instance.diffLevel == 2){
            if(PlayerPrefs.HasKey("mediumScore"))
                highScoreText.text = "Current highscore: " + PlayerPrefs.GetInt("mediumScore");
            else
                highScoreText.text = "YOU?";
        }
        else if(Difficulty.instance.diffLevel == 3){
            if(PlayerPrefs.HasKey("hardScore"))
                highScoreText.text = "Current highscore: " + PlayerPrefs.GetInt("hardScore");
            else
                highScoreText.text = "YOU?";
        }
        
        this.gameObject.SetActive(false);
    }

    void ToggleValueChanged(Toggle change){
        print("change toggle");
        int tempShake = PlayerPrefs.GetInt("ScreenShake");
        tempShake *= -1;
        PlayerPrefs.SetInt("ScreenShake", tempShake);
    }

    public void ChangeVolume(){
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    void LoadVolume(){
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        AudioListener.volume = volumeSlider.value;
    }

    void SaveVolume(){
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }



}
