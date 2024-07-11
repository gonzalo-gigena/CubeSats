using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosition: MonoBehaviour
{
    GameObject cubesat;
    GameObject sun;
    GameObject earth;
    const double SUN_RADIUS = 695700f; //(km) 
    const double EARTH_RADIUS = 6378.137f; //(km)
    const double UNIT = 100000f; // (km)
    List<Position> positions;
    int index = 0;

    void Start()
    {
        // Load Postions
        PositionLoader loader = new();
        positions = loader.LoadData();

        // Search objects by tag
        cubesat = GameObject.FindGameObjectWithTag("Cubesat");
        earth = GameObject.FindGameObjectWithTag("Earth");
        sun = GameObject.FindGameObjectWithTag("Sun");

        earth.transform.position = new Vector3(0, 0, 0);
        SetScale(earth, (float)(EARTH_RADIUS / UNIT));
        SetScale(sun, (float)(SUN_RADIUS / UNIT));

        UpdateSunAndCubesat();
    }

    void Update()
    {
        // Check if the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextPosition();
        }
    }

    void NextPosition(){
        index = (index + 1) % positions.Count;
        Debug.Log("Spacebar pressed! " + index);
        UpdateSunAndCubesat();
    }

    void UpdateSunAndCubesat()
    {
        Position currentPos = positions[index];

        Vector3 newSunPos = ListToVector3(currentPos.sun_pos);
        Vector3 newCubesatPos = ListToVector3(currentPos.sc_pos_i);

        sun.transform.position = newSunPos;
        cubesat.transform.position = newCubesatPos;

    }

    void SetScale(GameObject obj, float scale){
        obj.transform.localScale = new Vector3(scale, scale, scale);
    }

    Vector3 ListToVector3(List<double> list)
    {
        if (list.Count != 3)
        {
            throw new ArgumentException("The list must contain at least three elements.");
        }

        return new Vector3((float)(list[0] / UNIT), (float)(list[1] / UNIT), (float)(list[2] / UNIT));
    }


}