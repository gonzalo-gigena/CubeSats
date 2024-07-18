using System.Collections.Generic;
using UnityEngine;

public class UpdatePosition : MonoBehaviour
{
    Sun sun;
    Cubesat cubesat;
    Earth earth;
    List<Position> positions;
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
        index = (index + 1) % positions.Count;
        SetPositions();
    }

    void SetPositions()
    {
        Position currentPos = positions[index];

        List<double> subsolarPoint = currentPos.subsolar_point;

        sun.SetPosition(currentPos.sun_pos);
        earth.SetRotation(subsolarPoint, sun.GetBody());
        cubesat.SetPosition(currentPos.sc_pos_i);
        cubesat.LookAt(sun.GetBody());

    }

}