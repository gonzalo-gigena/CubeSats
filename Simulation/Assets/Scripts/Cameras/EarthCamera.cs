using UnityEngine;

public class EarthCamera : MonoBehaviour
{
    GameObject cubesat;  // The target object to follow
    GameObject earth;  // The target object to follow
    public float distance = 0.4f; // Distance from object1 to place the camera

    void Start()
    {
        cubesat = GameObject.FindGameObjectWithTag("Cubesat");
        earth = GameObject.FindGameObjectWithTag("Earth");
    }

    void LateUpdate()
    {
        // Calculate the direction from object1 to object2
        Vector3 direction = earth.transform.position - cubesat.transform.position;
        direction.Normalize();

        // Position the camera at the first object and offset it by the desired distance
        transform.position = cubesat.transform.position - direction * distance;

        // Make the camera look towards the second object
        transform.LookAt(earth.transform);
    }
}
