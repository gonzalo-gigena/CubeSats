using UnityEngine;

public class CubesatCamera : MonoBehaviour
{
    GameObject cubesat, earth;  // The target object to follow
    public float lookSpeed = 10f;  // Speed of looking around
    void Start()
    {
        cubesat = GameObject.FindGameObjectWithTag("Cubesat");
        earth = GameObject.FindGameObjectWithTag("Earth");
    }

    void LateUpdate()
    {
        if (transform.position != cubesat.transform.position){
            transform.position = cubesat.transform.position;
            transform.LookAt(earth.transform);
        }
        // Camera rotation
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;
            
            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }
    }
}
