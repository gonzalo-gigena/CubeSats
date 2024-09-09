using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    List<Camera> cameras;
    GameObject sunLight;
    LensFlareComponentSRP lensFlareSRP;
    int currentCam = 0;

    // Start is called before the first frame update
    void Start()
    {
        sunLight = GameObject.FindGameObjectWithTag("Sun Light");
        lensFlareSRP = sunLight.GetComponent<LensFlareComponentSRP>();

        cameras = GetCameras();
        NextCamera();
    }
    List<Camera> GetCameras()
    {
        List<Camera> cameras = new List<Camera>();
        foreach (Transform child in transform)
        {
            Camera cameraComponent = child.gameObject.GetComponent<Camera>();
            if (cameraComponent != null)
            {
                cameras.Add(cameraComponent);
            }
        }
        return cameras;
    }

    void NextCamera()
    {
        cameras[currentCam].gameObject.SetActive(true);
        for (var i = 0; i < cameras.Count; i++)
        {
            if (i != currentCam)
            {
                cameras[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentCam = (currentCam + 1) % cameras.Count;
            NextCamera();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentCam = (currentCam - 1 + cameras.Count) % cameras.Count;
            NextCamera();
        }
        // Check if the sun can be seen, if not disable flare.
        EnableFlareSRP();
    }

    void EnableFlareSRP()
    {
        lensFlareSRP.enabled = Flare.IsObjectInView(sunLight, cameras[currentCam]);
    }
}
