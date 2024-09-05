using UnityEngine;
using System.IO;
using System.Collections;

public class SatelliteCamera : MonoBehaviour
{
    Sun sun;  // The target object to follow
    Satellite satellite;

    string screenshotFolder = Path.Combine(Application.dataPath, "../../SyntheticImages");
    public float lookSpeed = 10f;  // Speed of looking around

    public void SetReferences(Satellite sat, Sun star)
    {
        satellite = sat;
        sun = star;
    }

    void Start()
    {
        // Create the screenshot folder if it doesn't exist
        if (!Directory.Exists(screenshotFolder))
        {
            Directory.CreateDirectory(screenshotFolder);
        }
    }

    void LateUpdate()
    {
        GameObject satBody = satellite.GetBody();
        GameObject sunBody = sun.GetBody();

        if (transform.position != satBody.transform.position)
        {
            transform.position = satBody.transform.position;
            transform.LookAt(sunBody.transform);
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
            StartCoroutine(CaptureScreenshot());
        }
    }

    private IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        // Create a Texture2D with screen width and height
        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // Read pixels from the screen into the texture
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        // Encode the resized texture to JPG with quality 93
        byte[] screenshotData = screenshotTexture.EncodeToJPG();

        // Define the path and file name for saving the screenshot
        string filePath = GenerateScreenshotPath();

        // Save the encoded JPG to the file
        File.WriteAllBytes(filePath, screenshotData);

        // Clean up memory
        Destroy(screenshotTexture);

        Debug.Log($"Screenshot taken and saved to: {filePath}");
    }

    string GenerateScreenshotPath()
    {
        Quaternion quaternion = transform.localRotation;
        string satRot = $"{quaternion.x},{quaternion.y},{quaternion.z},{quaternion.w}";
        string satPos = string.Join(",", satellite.originalPos);
        string filePath = $"{screenshotFolder}/{satellite.name}_{satellite.date}_{satPos}_{satRot}.jpg";
        
        return filePath;
    }
}
