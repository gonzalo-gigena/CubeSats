using System.Collections.Generic;
using UnityEngine;

public class Earth : Body
{
    protected GameObject child;
    public Earth(GameObject obj, double radius)
    {
        body = obj;
        SetScale(radius);
    }

    public void SetRotation(List<double> subsolarPoint, GameObject sun)
    {
        float lat = (float)subsolarPoint[0];
        float lon = (float)subsolarPoint[1];

        body.transform.LookAt(sun.transform.position);
        
        // Find a child by name
        GameObject child = body.transform.Find("Body").gameObject;
        child.transform.localRotation = Quaternion.Euler(0, -lon, -lat);
        

    }
}

