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

        points = new List<Vector2>(); // Our List of points that will make up our line

        points.Add(new Vector2(minPos.x, minPos.y + 1f)); // Left side of window
        points.Add(new Vector2(maxPos.x, minPos.y + 1f)); // Right side of window

        for (int i = 0; i < 5; i++)
        {
            generate(); // We want to expand our points 5 times!
        }

        KochLine = line(points); // We then show the line by passing the points into a LineRenderer
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
            Vector2 v = points[i+1] - points[i]; // This gives up the length of the line in 2 dimentions
            v = v / 3; // We only need 1/3rd of it since the line is split into thirds

            // First we need to define each new point from the original points in the line

            Vector2 a = points[i]; // This is the first point in the original line
            Vector2 b = v + a;  // 1/3rd after point A is where point B lies
            Vector2 c = Rotate(v, 60) + b; // Point C is 60 Degrees up from point B
            Vector2 d = (v * 2f) + a; // 2/3rds after point A is where point D lies
            Vector2 e = points[i+1]; // This is the last point in the original line

            // Now we add those points, in order, to our newPoint list

            newPoints.Add(a);
            newPoints.Add(b);
            newPoints.Add(c);
            newPoints.Add(d);
            newPoints.Add(e);
        }

        // After each line is added, we redefine the original points with the newPoints
        points = newPoints;
    }

    private GameObject line(List<Vector2> linePoints)
    {
        // We first generate a new object and give it a LineRenderer like in 8.4
        GameObject obj = new GameObject(); 
        obj.name = "Line";
        LineRenderer line = obj.AddComponent<LineRenderer>();

        // We give it a material and a color so we can see it
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = line.endColor = Color.black;

        // Let's give it a small width so we can see the complexity of the curve
        line.startWidth = line.endWidth = 0.1f;

        // We have to tell the LineRenderer to expect as many points as we have before we assign them
        line.positionCount = linePoints.Count;

        // We then can loop through those empty points and give them values
        int index = 0;
        foreach (Vector2 point in linePoints) {
            line.SetPosition(index, new Vector3(point.x, point.y, 0));
            index++;
        }

        return obj;
    }

    public Vector2 Rotate(Vector2 v, float degrees)
    {
        // Using some trigonometry, we take the vector and degrees to show us a new direction to point
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