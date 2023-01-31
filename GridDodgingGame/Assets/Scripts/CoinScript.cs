using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] float coinDestroyTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(coinDestroyTimer());
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
}
