using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject objectToOrbit;
    public Vector3 direction;
    public float angle;
    public float radius;
    public float degreesPerSecond = 10;

    public bool lookAtObject = false; 

    private void Start()
    {
        direction = (transform.position - objectToOrbit.transform.position).normalized;
        radius = Vector3.Distance(objectToOrbit.transform.position, transform.position);
    }

    private void Update()
    {
        if (lookAtObject)
        {
            this.transform.LookAt(objectToOrbit.transform);
        }
        angle += degreesPerSecond * Time.deltaTime;

        if (angle > 360)
        {
            angle -= 360;
        }

        Vector3 orbit = Vector3.forward * radius;
        Debug.Log(orbit);
        orbit = Quaternion.LookRotation(direction) * Quaternion.Euler(0, angle, 0) * orbit;

        transform.position = objectToOrbit.transform.position + orbit;
    }
}