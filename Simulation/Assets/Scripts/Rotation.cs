using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float hours = 24.0f;
    public float minutes = 60f;
    public float seconds = 60f;
    public bool rotate = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!rotate) return;

        float deltaAngle = 360.0f / (hours * minutes * seconds) * Time.deltaTime; 
        this.transform.RotateAround(
            this.transform.position,
            Vector3.up,
            deltaAngle
        );
    }
}
