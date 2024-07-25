using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    List<GameObject> cameras;
    int currentCam = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameras = GetChildren();
        NextCamera();
    }

    // Update is called once per frame
    void LateUpdate()
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
    }

    public List<GameObject> GetChildren()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }

    void NextCamera()
    {
        cameras[currentCam].SetActive(true);
        for (var i = 0; i < cameras.Count; i++)
        {
            if (i != currentCam)
            {
                cameras[i].SetActive(false);
            }
        }
    }
}
