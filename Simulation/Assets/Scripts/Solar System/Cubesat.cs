using UnityEngine;

public class Cubesat : Body
{

    // Start is called before the first frame update
    public Cubesat(GameObject obj)
    {
        body = obj;
    }

    public void LookAt(GameObject obj){
        body.transform.LookAt(obj.transform);
    }
}
