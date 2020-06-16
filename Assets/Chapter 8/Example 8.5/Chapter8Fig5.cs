using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter8Fig5 : MonoBehaviour
{
    Vector2 minPos, maxPos;
    List<Vector2> points;
    GameObject KochLine;
    // Start is called before the first frame update
    void Start()
    {
        findWindowLimits();

        points = new List<Vector2>();

        points.Add(new Vector2(minPos.x, minPos.y + 1f)); // Left side of window
        points.Add(new Vector2(maxPos.x, minPos.y + 1f)); // Right side of window

        for (int i = 0; i < 5; i++)
        {
            generate();
        }

        KochLine = line(points);
    }

    void generate() 
    {
        if (KochLine != null)  // We're going to replace the line!
        {
            Destroy(KochLine);
        }

        List<Vector2> newPoints = new List<Vector2>();

        for (int i = 0; i < points.Count - 1; i++) // We're going to use 2 points each time to make a line
        {
            Vector2 v = points[i+1] - points[i];
            v = v / 3;

            Vector2 a = points[i];
            Vector2 b = v + a;
            Vector2 c = Rotate(v, 60) + b;
            Vector2 d = (v * 2f) + a;
            Vector2 e = points[i+1];

            newPoints.Add(a);
            newPoints.Add(b);
            newPoints.Add(c);
            newPoints.Add(d);
            newPoints.Add(e);
        }

        points = newPoints;
    }

    private GameObject line(List<Vector2> linePoints)
    {
        GameObject obj = new GameObject();
        obj.name = "Line";
        LineRenderer line = obj.AddComponent<LineRenderer>();

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = line.endColor = Color.black;

        line.startWidth = line.endWidth = 0.1f;

        line.positionCount = linePoints.Count;

        int index = 0;
        foreach (Vector2 point in linePoints) {
            line.SetPosition(index, new Vector3(point.x, point.y, 0));
            index++;
        }

        return obj;
    }

    public Vector2 Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
        minPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maxPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}