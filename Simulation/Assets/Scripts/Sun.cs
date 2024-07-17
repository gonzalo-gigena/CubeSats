using UnityEngine;

public class Sun : Body
{
    // Start is called before the first frame update
    public Sun(GameObject obj, double radius)
    {
        body = obj;
        SetScale(radius);
    }
}
