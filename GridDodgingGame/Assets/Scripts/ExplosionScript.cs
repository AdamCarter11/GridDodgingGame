using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{

    bool canTrigger = true;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("enemy") && canTrigger){
            canTrigger = false;
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(explosionDur());
        }
    }
    IEnumerator explosionDur(){
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

}
