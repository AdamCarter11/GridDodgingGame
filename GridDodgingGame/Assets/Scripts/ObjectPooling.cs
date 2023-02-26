using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int poolCap = 30;

    [SerializeField] private GameObject ratPrefab;
    
    private void Awake() {
        if(instance == null){
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < poolCap; i++){
            GameObject obj = Instantiate(ratPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject(){
        for(int i = 0; i < pooledObjects.Count; i++){
            if(!pooledObjects[i].activeInHierarchy){
                return pooledObjects[i];
            }
        }
        return null;
    }
}
