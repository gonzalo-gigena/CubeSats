using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosition : MonoBehaviour
{
    Sun sun;
    Satellite sat;
    Earth earth;
    Positions positions;
    SatelliteCamera satelliteCameraScript;
    int index = 0;
    int satellite_index = 0; // for now there is only one satellite 

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
        sat = new Satellite(cubesatObj);

        // Pass reference to satellite camera
        GameObject satelliteCamera = GameObject.Find("SatelliteCamera");
        satelliteCameraScript = satelliteCamera.GetComponent<SatelliteCamera>();
        satelliteCameraScript.SetReferences(sat);

        StartCoroutine(CaptureScreenshotsContinuously());
    }

    // Coroutine to capture screenshots continuously
    IEnumerator CaptureScreenshotsContinuously()
    {
        while (index < positions.total)
        {
            // Move to the next position of the planets
            SetPositions();

            // Take 5 screenshots for each position
            for (int shotCount = 0; shotCount < 10; shotCount++)
            {
                // Randomize the camera's rotation for each shot
                satelliteCameraScript.RandomizeCameraRotation();

                // Take a screenshot (wait for end of frame to ensure proper rendering)
                yield return StartCoroutine(satelliteCameraScript.CaptureScreenshot());

                // Optionally, yield return null to capture the next frame immediately
                yield return null;
            }

            // Increment the image index after all 5 screenshots are taken
            index++;
        }

        Debug.Log("Screenshot capture complete.");
    }

    /*void Update()
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
        index = (index + 1) % positions.total;
        Debug.Log($"Position: {index}");
        SetPositions();
    }*/

    void SetPositions()
    {
        string date = positions.dates[index];
        List<double> subsolarPoint = positions.subsolar_points[index];
        List<double> sunPosition = positions.sun_pos[index];

        string name = positions.satellites[satellite_index].name;
        List<double> satPosition = positions.satellites[satellite_index].pos[index];

        sun.SetPosition(sunPosition);
        earth.SetRotation(subsolarPoint, sun.GetBody());

        sat.SetPosition(satPosition);
        //sat.LookAt(sun.GetBody());
        sat.UpdateProperties(date, name, satPosition);
    }

}