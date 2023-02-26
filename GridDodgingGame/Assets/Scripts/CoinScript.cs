using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] float coinDestroyTime;
    //controls when the coin starts flashing
    [SerializeField] float coinFlashThreshold;
    float flashDelay = .5f, flashingVar;
    bool onOff = true;
    // Start is called before the first frame update
    void Start()
    {
        flashingVar = coinDestroyTime;
        StartCoroutine(coinDestroyTimer());
        StartCoroutine(FadeInOut());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Wall")){
            Destroy(this.gameObject);
        }
    }

    IEnumerator coinDestroyTimer(){
        yield return new WaitForSeconds(coinDestroyTime);
        Destroy(this.gameObject);
    }
    IEnumerator FadeInOut(){
        Color tempColor = this.gameObject.GetComponent<SpriteRenderer>().color;
        while(true){
            if(flashingVar < coinFlashThreshold){
                if(onOff){
                    tempColor.a = .5f;
                    this.gameObject.GetComponent<SpriteRenderer>().color = tempColor;
                    onOff = false;
                }else{
                    tempColor.a = 1f;
                    this.gameObject.GetComponent<SpriteRenderer>().color = tempColor;
                    onOff = true;
                }
                if(flashDelay > .3f){
                    flashDelay -= .1f;
                }
            }
            yield return new WaitForSeconds(flashDelay);
            flashingVar -= flashDelay;
        }
    }
}
