using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    GameObject sun;
    GameObject earth;
    GameObject cubesat;

    Vector3 earth_offset;
    Vector3 cubesat_offset;


    [SerializeField]
    float earth_speed = 1f;
    float cubesat_speed = 100f;
    // Start is called before the first frame update
    void Start()
    {

        cubesat = GameObject.FindGameObjectWithTag("Cubesat");
        earth = GameObject.FindGameObjectWithTag("Earth");
        sun = GameObject.FindGameObjectWithTag("Sun");


        earth_offset = earth.transform.position - sun.transform.position;
        cubesat_offset = cubesat.transform.position - earth.transform.position;
        /*
            SetInitialParams(
                earth,
                Units.Velocity(Units.EARTH_VELOCITY),
                1f,
                1f,
                Units.Earth_pos()
            );
            SetInitialParams(sun,
                Units.Velocity(Units.SUN_VELOCITY),
                Units.Mass(Units.SUN_MASS),
                Units.Scale(Units.SUN_RADIUS),
                Units.Sun_pos()
            );
            SetInitialParams(cubesat,
                Units.Velocity(Units.CUBESAT_VELOCITY),
                Units.Mass(Units.CUBESAT_MASS),
                Units.CUBESAT_RADIUS,
                Units.Cubesat_pos()
            );

            celestials.Add(earth);
            celestials.Add(sun);
            celestials.Add(cubesat);

            //SetInitialVelocity();
            */

    }

    void SetInitialParams(GameObject obj, float velocity, float mass, float scale, Vector3 pos)
    {
        obj.transform.localScale = new Vector3(scale, scale, scale);
        obj.transform.position = pos;

        Debug.Log(pos);
        obj.GetComponent<Rigidbody>().velocity += obj.transform.right * velocity;
        obj.GetComponent<Rigidbody>().mass = mass;
    }

    void Orbit(GameObject a, GameObject b, Vector3 offset, float speed)
    {
        // Rotate around the b's position
        a.transform.RotateAround(b.transform.position, Vector3.up, speed * Time.deltaTime);
        // Calculate the new offset (distance vector) after rotation
        Vector3 currentOffset = transform.position - sun.transform.position;

        // Maintain the initial distance magnitude
        transform.position = sun.transform.position + offset.normalized * currentOffset.magnitude;
    }

    void Orbits()
    {
        //Orbit(earth, sun, earth_offset, earth_speed);
        //Orbit(cubesat, earth, cubesat_offset, cubesat_speed);
    }

    // Update is called once per frame
    void Update()
    {
        Orbits();
    }
}