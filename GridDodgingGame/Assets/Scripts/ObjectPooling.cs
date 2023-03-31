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
        if (instance == null) {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolCap; i++) {
            GameObject obj = Instantiate(ratPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject() {
        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy) {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public Vector3 GetTrajectoryGameObject(GameObject sourceObject)
    {
        Vector3 sourcePosition = sourceObject.transform.position;
        // Return sourceObject position if no rat with trajectory to collide
        return Vector3.zero;
    }

    public Vector3 GetClosestGameObject(GameObject sourceObject)
    {
        // Source position to compare to
        Vector3 sourcePosition = sourceObject.transform.position;

        // Closest object to source
        Vector3 closestPosition = new Vector3(float.MaxValue, float.MaxValue);

        // Set lowest distance to max
        float lowestDistance = float.MaxValue;

        Vector3 tempPosition;
        float currentDistance;

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                // Assign position to temp
                tempPosition = pooledObjects[i].transform.position;

                // Distance between vectors
                //  d^2 = (x - x0)^2 + (y - y0)^2
                currentDistance = Mathf.Pow(tempPosition.x - sourcePosition.x, 2) +
                    Mathf.Pow(tempPosition.y - sourcePosition.y, 2);
                currentDistance = Mathf.Pow(currentDistance, 2);

                if (currentDistance < lowestDistance)
                {
                    lowestDistance = currentDistance;
                    closestPosition = tempPosition;
                }
            }
        }

        // Return original position if no other rats to collide with
        if (lowestDistance != float.MaxValue)
        {
            return closestPosition;
        }
        else return sourcePosition;
    }
}
