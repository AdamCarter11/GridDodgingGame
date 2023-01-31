using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
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
    }

    public void ChangeVolume(){
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    void LoadVolume(){
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    void SaveVolume(){
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

}
