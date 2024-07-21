using System;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosition : MonoBehaviour
{
    Sun sun;
    Cubesat cubesat;
    Earth earth;
    Positions positions;
    int index = 0;

    void Start()
    {
        // Load Postions
        PositionLoader loader = new();
        positions = loader.LoadData();

        // Search objects by tag
        GameObject cubesatObj = GameObject.FindGameObjectWithTag("Cubesat");
        GameObject earthObj = GameObject.FindGameObjectWithTag("Earth");
        GameObject sunObj = GameObject.FindGameObjectWithTag("Sun");

        earth = new Earth(earthObj, Units.EARTH_RADIUS);
        sun = new Sun(sunObj, Units.SUN_RADIUS);
        cubesat = new Cubesat(cubesatObj);

        SetPositions();
    }

    void Update()
    {
        // Check if the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextPosition();
        }
    }

    void NextPosition()
    {
        // All list inside the Positions object have the same length
        index = (index + 1) % positions.dates.Count;
        SetPositions();
    }

    void SetPositions()
    {
        String date = positions.dates[index];
        List<double> subsolarPoint = positions.subsolar_points[index];
        List<double> sunPosition = positions.sun_pos[index]; 

        sun.SetPosition(sunPosition);
        earth.SetRotation(subsolarPoint, sun.GetBody());
    
        foreach(Satellite satellite in positions.satellites){
            cubesat.SetPosition(satellite.pos[index]);
            cubesat.LookAt(sun.GetBody());
        }


    }

}