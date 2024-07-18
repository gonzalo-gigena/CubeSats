using System;
using System.Collections.Generic;
using UnityEngine;

public class Body
{
    protected GameObject body;

    public GameObject GetBody()
    {
        return body;
    }

    public void SetScale(double scale)
    {
        float newScale = (float)(scale / Units.UNIT);
        body.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void SetPosition(List<double> list)
    {
        Vector3 newPos = ListToVector3(list);
        body.transform.position = newPos;
    }

    private Vector3 ListToVector3(List<double> list)
    {
        if (list.Count != 3)
        {
            throw new ArgumentException("The list must contain at least three elements.");
        }

        return new Vector3((float)(list[0] / Units.UNIT), (float)(list[1] / Units.UNIT), (float)(list[2] / Units.UNIT));
    }
}
