using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle shakeToggle;
    [SerializeField] Toggle invertControlToggle;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] GameObject arrows, digButton;
    Button testButton;
    [HideInInspector] public static bool toggleFlipControls;
    // Start is called before the first frame update
    void Start()
    {
        toggleFlipControls = false;
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

        //invert controls
        if (!PlayerPrefs.HasKey("InvertControls"))
        {
            PlayerPrefs.SetInt("InvertControls", -1);
        }
        else
        {
            if (PlayerPrefs.GetInt("InvertControls") == 1)
            {
                invertControlToggle.isOn = true;
                toggleFlipControls = true;
                //arrows.GetComponent<RectTransform>().anchoredPosition = new Vector3(720, 0, 0);
                arrows.GetComponent<RectTransform>().anchorMin = new Vector2(1,0);
                arrows.GetComponent<RectTransform>().anchorMax = new Vector2(1,0);
                arrows.GetComponent<RectTransform>().anchoredPosition = new Vector3(-150, 0, 0);

                //digButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-790, 1, 0);
                digButton.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
                digButton.GetComponent<RectTransform>().anchorMax = new Vector2(0,0);
                digButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(125, 1, 0);
            }
            else
            {
                invertControlToggle.isOn = false;
                toggleFlipControls = false;
                arrows.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                arrows.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
                arrows.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

                digButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                digButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                digButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-20, 1, 0);
            }
        }
        invertControlToggle.onValueChanged.AddListener(delegate { ToggleAltControlValueChanged(invertControlToggle); });

        if (Difficulty.instance.diffLevel == 1){
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

    void ToggleAltControlValueChanged(Toggle change)
    {
        print("change alt controls toggle");
        int tempAltControl = PlayerPrefs.GetInt("InvertControls");
        tempAltControl *= -1;
        PlayerPrefs.SetInt("InvertControls", tempAltControl);
        if(tempAltControl == 1)
        {
            toggleFlipControls = true;
            //arrows.GetComponent<RectTransform>().anchoredPosition = new Vector3(650, 0, 0);
            //digButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-677, 1, 0);

            arrows.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            arrows.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
            arrows.GetComponent<RectTransform>().anchoredPosition = new Vector3(-150, 0, 0);

            //digButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-790, 1, 0);
            digButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            digButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            digButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(125, 1, 0);
        }
        else
        {
            toggleFlipControls = false;
            //arrows.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            //digButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-20, 1, 0);
            arrows.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            arrows.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            arrows.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

            digButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            digButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
            digButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-20, 1, 0);
        }
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
