using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FlareManager : MonoBehaviour
{
    GameObject sunLight;
    LensFlareComponentSRP lensFlareSRP;

    // Start is called before the first frame update
    void Start()
    {
        sunLight = GameObject.FindGameObjectWithTag("Sun Light");
        lensFlareSRP = sunLight.GetComponent<LensFlareComponentSRP>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the sun can be seen, if not disable flare.
        EnableFlareSRP();
    }

    void EnableFlareSRP()
    {
        // Get the camera attached to the current GameObject (this script is attached to the camera)
        Camera camera = GetComponent<Camera>();
        lensFlareSRP.enabled = IsObjectInView(sunLight, camera);
    }

    public static bool IsObjectInView(GameObject obj, Camera cam)
    {
        // Convert the object's position to viewport space
        Vector3 viewportPoint = cam.WorldToViewportPoint(obj.transform.position);

        // Check if the viewport point is within the camera's view frustum
        bool isInViewport = viewportPoint.x >= -0.5 && viewportPoint.x <= 1.5 &&
                            viewportPoint.y >= -0.5 && viewportPoint.y <= 1.5 &&
                            viewportPoint.z > 0;

        if (!isInViewport)
            return false;

        // Perform a raycast to check if there's any obstruction
        Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(obj.transform.position));
        RaycastHit hit;

        // Check if the ray hits any object before reaching the target object
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject != obj)
            {
                return false; // There's something in between
            }
        }

        return true; // Object is visible and not blocked
    }
}
