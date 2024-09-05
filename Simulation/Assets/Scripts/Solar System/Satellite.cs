using UnityEngine;
using System.Collections.Generic;
using System;

public class Satellite : Body
{   
    public List<double> originalPos;
    public string name;
    public string date;

    // Start is called before the first frame update
    public Satellite(GameObject obj)
    {
        body = obj;
    }

    public void LookAt(GameObject obj){
        body.transform.LookAt(obj.transform);
    }

    public void UpdateProperties(string satDate, string satName, List<double> satPosition){
        date = satDate;
        name = satName;
        originalPos = satPosition;
    }
}
