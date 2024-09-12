using UnityEngine;

public static class Flare
{
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
