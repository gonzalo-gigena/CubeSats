using UnityEngine;

public class FreeCameraMovement : MonoBehaviour
{
    GameObject cubesat;  // The target object to follow
    public float moveSpeed = 10f; // Speed of movement
    public float lookSpeed = 2f;  // Speed of looking around
    public float zoomSpeed = 10f; // Speed of zooming
    void Start()
    {
        cubesat = GameObject.FindGameObjectWithTag("Cubesat");
        transform.position = cubesat.transform.position;
    }

    void LateUpdate()
    {
        // Camera rotation
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;
            
            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }

        // Camera movement
        float moveForward = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float moveRight = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveUp = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            moveUp = moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            moveUp = -moveSpeed * Time.deltaTime;
        }

        Vector3 move = transform.right * moveRight + transform.forward * moveForward + transform.up * moveUp;
        transform.position += move;

        // Camera zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        transform.position += transform.forward * scroll;
    }
}
