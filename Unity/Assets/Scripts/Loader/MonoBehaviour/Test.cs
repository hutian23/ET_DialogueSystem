using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform StartPoint;
    public Transform MidPoint;
    public Transform EndPoint;
    public Transform Sphere;
    private int speed = 2;
    private float percentSpeed;//百分比速度
    private float percent = 0;//路径百分比
    void Start()
    {
        percent = 0;
        percentSpeed = speed / (EndPoint.position - StartPoint.position).magnitude;
    }

    [Button("Test")]
    public void ButtonTest()
    {
        this.percent = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (percent >= 1)
        {
            return;
        }

        percent += percentSpeed * Time.deltaTime;   
        if (percent > 1)
            percent = 1;

        Sphere.position = Bezier(percent, StartPoint.position, MidPoint.position, EndPoint.position);
    }

    private static Vector3 Bezier(float t, Vector3 a,Vector3 b,Vector3 c)
    {
        var ab = Vector3.Lerp(a,b,t);
        var bc = Vector3.Lerp(b,c,t);
        return Vector3.Lerp(ab,bc,t);
    }
}