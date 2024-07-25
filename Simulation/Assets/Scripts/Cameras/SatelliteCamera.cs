using UnityEngine;

public class SatelliteCamera : MonoBehaviour
{
    GameObject satellite, sun;  // The target object to follow
    string screenshotFolder = System.IO.Path.Combine(Application.dataPath, "../../SyntheticImages");
    public float lookSpeed = 10f;  // Speed of looking around
    void Start()
    {
        // Create the screenshot folder if it doesn't exist
        if (!System.IO.Directory.Exists(screenshotFolder))
        {
            System.IO.Directory.CreateDirectory(screenshotFolder);
        }

        satellite = GameObject.FindGameObjectWithTag("Cubesat");
        sun = GameObject.FindGameObjectWithTag("Sun");
    }

    void LateUpdate()
    {
        if (transform.position != satellite.transform.position)
        {
            transform.position = satellite.transform.position;
            transform.LookAt(sun.transform);
        }
        // Camera rotation
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }
        // Check for the screenshot key press
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeScreenshot();
        }
    }

    public void TakeScreenshot()
    {
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string position = satellite.transform.position.ToString();
        string quaternion = transform.localRotation.ToString();
        string filename = $"{screenshotFolder}/{timestamp}-{position}-{quaternion}.png";

        ScreenCapture.CaptureScreenshot(filename);
        Debug.Log($"Screenshot taken and saved to: {filename}");
    }
}
