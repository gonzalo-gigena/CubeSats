using UnityEngine;

public class Camera : MonoBehaviour
{
    GameObject cubesat;  // The target object to follow
    GameObject earth;  // The target object to follow
    public Vector3 offset;  // The offset distance between the camera and the target
    public float distance = 2.0f; // Distance from object1 to place the camera

    void Start()
    {
        cubesat = GameObject.FindGameObjectWithTag("Cubesat");
        earth = GameObject.FindGameObjectWithTag("Earth");

        if (cubesat != null)
        {
            // Calculate and store the initial offset (distance vector) between the camera and the target
            offset = transform.position - cubesat.transform.position;
        }
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

    void LateUpdatesdsada()
    {
        if (cubesat != null)
        {
            // Update the camera's position to maintain the offset from the target
            transform.position = cubesat.transform.position + offset;

            // Optionally, you can make the camera look at the target
            transform.LookAt(cubesat.transform);
        }
    }
}
