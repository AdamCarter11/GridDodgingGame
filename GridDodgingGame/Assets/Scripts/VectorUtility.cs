using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorUtility : MonoBehaviour
{
    public static VectorUtility Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null || Instance != null)
        {
            Destroy(Instance);
            Instance = this;
        }
    }

    // Calculate distance between vectors
    //  d^2 = (x - x0)^2 + (y - y0)^2
    public float calculateDistance(Vector3 origin, Vector3 destination)
    {
        float currentDistance;

        currentDistance = Mathf.Pow(destination.x - origin.x, 2) + Mathf.Pow(origin.y - origin.y, 2);
        currentDistance = Mathf.Pow(currentDistance, 2);

        return currentDistance;
    }

    // Return as unit vector for which direction to face in a xy-coordinate grid
    public Vector3 calculateFacing(Vector3 origin, Vector3 destination)
    {
        Vector3 directionVector;

        float xDiff, yDiff;
        xDiff = Mathf.Abs(destination.x - origin.x);
        yDiff = Mathf.Abs(destination.y - origin.y);

        if (xDiff > yDiff)
        {
            if (destination.x - origin.x > 0)
            {
                directionVector = Vector3.left;
            }
            else
            {
                directionVector = Vector3.right;
            }
        }
        else
        {
            if (destination.y - origin.y > 0)
            {
                directionVector = Vector3.up;
            }
            else
            {
                directionVector = Vector3.down;
            }
        }

        return directionVector;
    }
}
