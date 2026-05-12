using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    // 1~2
    public Transform point0;
    //public Transform point1;
    //public Transform point2;
    public Transform point3;

    //3
    //public List<Transform> points = new List<Transform>();
    //List<Vector3> pointPositions = new List<Vector3>();

    [Header("RandomSetting")]
    public float p1Radius = 2f;
    public float p2Radius = 2f;
    public float p1Height = 3f;
    public float p2Height = 3f;

    [HideInInspector] public Vector3 p1;
    [HideInInspector] public Vector3 p2;

    List<Vector3> points;
    float timeValue = 0f;

    private void Awake()
    {
        //foreach (var pt in points)
        //{
        //    if (pt != null)
        //    {
        //        pointPositions.Add(pt.position);
        //    }
        //}
    }

    void Update()
    {
        //timeValue += Time.deltaTime;
        //transform.position = GetPointOnBezierCurve(point0.position, point1.position, point2.position, point3.position, timeValue);

        
        timeValue += Time.deltaTime / 2f;
        transform.position = DeCasteljau(points, timeValue);
    }

    public void StartShooting()
    {
        GenerateRandomControlPoints();
        points = new List<Vector3> { point0.position, p1, p2, point3.position };
    }

    Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        Vector3 abc = Vector3.Lerp(ab, bc, t);

        return abc;
    }

    Vector3 DeCasteljau(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;

            var next = new List<Vector3>(last);
            for (int i = 0; i < last; i++)
            {
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            }
            p = next;
        }

        return p[0];
    }

    void GenerateRandomControlPoints()
    {
        Vector2 rand1 = Random.insideUnitCircle * p1Radius;
        p1 = point0.position + new Vector3(rand1.x, 0f, rand1.y);
        p1.y += p1Height;

        Vector2 rand2 = Random.insideUnitCircle * p2Radius;
        p2 = point3.position + new Vector3(rand2.x, 0f, rand2.y);
        p2.y += p2Height;
    }
}
