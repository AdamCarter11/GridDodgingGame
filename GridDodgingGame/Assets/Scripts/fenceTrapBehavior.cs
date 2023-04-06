using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fenceTrapBehavior : MonoBehaviour
{
    [SerializeField] Sprite activeTrapSprite;
    private bool isFenceEnabled;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.numFenceTraps++;
    }

    public void SwapSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = activeTrapSprite;
    }

    private void OnDestroy()
    {
        GameManager.Instance.numFenceTraps--;
    }
}
