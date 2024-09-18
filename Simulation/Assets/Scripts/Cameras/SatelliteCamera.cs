using UnityEngine;
using System.IO;
using System.Collections;

public class SatelliteCamera : MonoBehaviour
{
    Satellite satellite;

    string screenshotFolder = Path.Combine(Application.dataPath, "../../SyntheticImages");
    public float lookSpeed = 10f;  // Speed of looking around

    public void SetReferences(Satellite sat)
    {
        satellite = sat;
    }

    // Function to randomize the camera's rotation
    public void RandomizeCameraRotation()
    {
        // Get the camera attached to the current GameObject (this script is attached to the camera)
        Camera camera = GetComponent<Camera>();

        // Generate random angles for x, y, and z axes
        float randomX = Random.Range(0f, 360f);
        float randomY = Random.Range(0f, 360f);
        float randomZ = Random.Range(0f, 360f);

        // Apply random rotation to the camera
        camera.transform.rotation = Quaternion.Euler(randomX, randomY, randomZ);
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

        if (transform.position != satBody.transform.position)
        {
            transform.position = satBody.transform.position;
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
    public IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        // Get the camera attached to the current GameObject (this script is attached to the camera)
        Camera camera = GetComponent<Camera>();

        // Render at a higher resolution (e.g., 1920x1920 or any resolution with more detail)
        int highResWidth = 1280;
        int highResHeight = 1280;
        RenderTexture highResRenderTexture = new RenderTexture(highResWidth, highResHeight, 24);
        RenderTexture.active = highResRenderTexture;

        // Create a high-resolution Texture2D
        Texture2D highResScreenshotTexture = new Texture2D(highResWidth, highResHeight, TextureFormat.RGB24, false);

        // Capture the screen into the high-resolution RenderTexture
        camera.targetTexture = highResRenderTexture;
        camera.Render();

        // Read pixels from the high-resolution RenderTexture into the Texture2D
        highResScreenshotTexture.ReadPixels(new Rect(0, 0, highResWidth, highResHeight), 0, 0);
        highResScreenshotTexture.Apply();

        // Downsample the image using bilinear interpolation
        Texture2D lowResTexture = DownsampleBilinear(highResScreenshotTexture, 102, 102);

        // Encode the downscaled texture to JPG with quality 93
        byte[] screenshotData = lowResTexture.EncodeToJPG(93);

        // Define the path and file name for saving the screenshot
        string filePath = GenerateScreenshotPath();

        // Save the encoded JPG to the file
        File.WriteAllBytes(filePath, screenshotData);

        // Clean up memory
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(highResRenderTexture);
        Destroy(highResScreenshotTexture);
        Destroy(lowResTexture);

        Debug.Log($"Screenshot taken and saved to: {filePath}");
    }

    // Bilinear interpolation downsampling function
    private Texture2D DownsampleBilinear(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, false);

        float ratioX = (float)source.width / targetWidth;
        float ratioY = (float)source.height / targetHeight;

        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                // Calculate the position in the source texture
                float srcX = x * ratioX;
                float srcY = y * ratioY;

                // Get the surrounding pixels for interpolation
                int x1 = Mathf.FloorToInt(srcX);
                int y1 = Mathf.FloorToInt(srcY);
                int x2 = Mathf.Min(x1 + 1, source.width - 1);
                int y2 = Mathf.Min(y1 + 1, source.height - 1);

                // Get the fractional parts of the pixel positions
                float tX = srcX - x1;
                float tY = srcY - y1;

                // Get the pixel colors from the four neighboring pixels
                Color topLeft = source.GetPixel(x1, y1);
                Color topRight = source.GetPixel(x2, y1);
                Color bottomLeft = source.GetPixel(x1, y2);
                Color bottomRight = source.GetPixel(x2, y2);

                // Bilinear interpolation
                Color top = Color.Lerp(topLeft, topRight, tX);
                Color bottom = Color.Lerp(bottomLeft, bottomRight, tX);
                Color finalColor = Color.Lerp(top, bottom, tY);

                // Set the pixel in the result texture
                result.SetPixel(x, y, finalColor);
            }
        }

        result.Apply();
        return result;
    }


    private IEnumerator CaptureScreenshot1()
    {
        yield return new WaitForEndOfFrame();

        // Get the camera attached to the current GameObject (this script is attached to the camera)
        Camera camera = GetComponent<Camera>();

        // Render at a higher resolution (e.g., 1920x1920 or any resolution with more detail)
        int highResWidth = 1920;
        int highResHeight = 1920;
        RenderTexture highResRenderTexture = new RenderTexture(highResWidth, highResHeight, 24);
        RenderTexture.active = highResRenderTexture;

        // Create a high-resolution Texture2D
        Texture2D highResScreenshotTexture = new Texture2D(highResWidth, highResHeight, TextureFormat.RGB24, false);

        // Capture the screen into the high-resolution RenderTexture
        camera.targetTexture = highResRenderTexture;
        camera.Render();

        // Read pixels from the high-resolution RenderTexture into the Texture2D
        highResScreenshotTexture.ReadPixels(new Rect(0, 0, highResWidth, highResHeight), 0, 0);
        highResScreenshotTexture.Apply();

        // Manually downscale the high-res image to 102x102
        Texture2D lowResTexture = new Texture2D(102, 102, TextureFormat.RGB24, false);
        Color[] pixels = highResScreenshotTexture.GetPixels(0, 0, highResWidth, highResHeight);
        Color[] resizedPixels = new Color[102 * 102];

        // Simple downscaling algorithm (you can improve this with bilinear or bicubic interpolation)
        float ratioX = (float)highResWidth / 102;
        float ratioY = (float)highResHeight / 102;
        for (int y = 0; y < 102; y++)
        {
            for (int x = 0; x < 102; x++)
            {
                int newX = Mathf.FloorToInt(x * ratioX);
                int newY = Mathf.FloorToInt(y * ratioY);
                resizedPixels[y * 102 + x] = pixels[newY * highResWidth + newX];
            }
        }

        lowResTexture.SetPixels(resizedPixels);
        lowResTexture.Apply();

        // Encode the downscaled texture to JPG with quality 93
        byte[] screenshotData = lowResTexture.EncodeToJPG(93);

        // Define the path and file name for saving the screenshot
        string filePath = GenerateScreenshotPath();

        // Save the encoded JPG to the file
        File.WriteAllBytes(filePath, screenshotData);

        // Clean up memory
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(highResRenderTexture);
        Destroy(highResScreenshotTexture);
        Destroy(lowResTexture);

        Debug.Log($"Screenshot taken and saved to: {filePath}");
    }

    string GenerateScreenshotPath()
    {
        Quaternion quaternion = transform.rotation;
        string satRot = $"{quaternion.x},{quaternion.y},{quaternion.z},{quaternion.w}";
        string satPos = string.Join(",", satellite.originalPos);
        string filePath = $"{screenshotFolder}/{satellite.name}_{satellite.date}_{satPos}_{satRot}.jpg";

        return filePath;
    }
}
