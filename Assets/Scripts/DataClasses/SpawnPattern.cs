using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPattern
{
    public List<Vector2> listObjects;

    public Vector3 LeftRightTopBound()
    {
        Vector3 result = new Vector3(listObjects[0].x, listObjects[0].x, listObjects[0].y);
        Vector2 currentObj;
        for (int i = 1; i < listObjects.Count; i++)
        {
            currentObj = listObjects[i];
            if (currentObj.x < result.x)
            {
                result.x = currentObj.x;
            } else if(currentObj.x > result.y)
            {
                result.y = currentObj.x;
            }

            if(currentObj.y > result.z)
            {
                result.z = currentObj.y;
            }
        }

        return result;
    }

    public float LowerBound()
    {
        float lower = listObjects[0].y;
        float currentY;
        for(int i = 1; i < listObjects.Count; i++)
        {
            currentY = listObjects[i].y;
            if(currentY < lower)
            {
                lower = currentY;
            }
        }

        return lower;
    }
}
