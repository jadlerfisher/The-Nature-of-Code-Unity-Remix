using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter8Fig5Animated : MonoBehaviour
{
    Vector2 minPos, maxPos;
    List<Vector2> points;
    List<Vector2> desiredPoints;
    GameObject KochLine;

    int calls = 0;
    // Start is called before the first frame update
    void Start()
    {
        findWindowLimits();

        points = new List<Vector2>();
        desiredPoints = new List<Vector2>(); // We need a desired set to move towards in the curve

        Vector2 a = new Vector2(minPos.x, minPos.y + 1f);
        Vector2 b = new Vector2(maxPos.x, minPos.y + 1f);

        desiredPoints.Add(a);
        desiredPoints.Add(b);

        points.Add(Vector2.zero); // for a nicer starting effect, we'll start the line at 0,0
        points.Add(Vector2.zero);

        StartCoroutine(itterate()); // We can then start a Coroutine to do the generations over time
    }

    private IEnumerator itterate() 
    {
        while (calls < 6) // We have to limit the number of times we call generate() otherwise the LineRenderer will reach max points
        {
            yield return new WaitForSeconds(5f); // After 5 seconds, the function will continue
            generate();
            calls++;
        }
    }

    private void Update()
    {
        if (KochLine != null) 
        {
            Destroy(KochLine); // If the KochLine exsists, we're goint to replace it
        }

        for (int i = 0; i < desiredPoints.Count; i++) 
        {
            Vector2 desired = desiredPoints[i];
            Vector2 at = points[i];

            // For each point, we move the current point towards the desitred point by 30% every second.

            points[i] += ((desired - at) / 1.3f) * Time.deltaTime; // Time.deltaTime will help convert the 'Per Frame' speed of the Update Function to a 'Per Second' one
        }

        KochLine = line(points); // We then regenerate the line
    }

    void generate()
    {
        List<Vector2> newPoints = new List<Vector2>();
        List<Vector2> cutPoints = new List<Vector2>();

        for (int i = 0; i < desiredPoints.Count - 1; i++) // We're going to use 2 points each time to make a line
        {
            // First we make sure to add the points to the real line so they can be animated
            Vector2 v = points[i + 1] - points[i];

            cutPoints.Add(points[i]); // We keep the first point
            cutPoints.Add(points[i] + ((v / 4) * 1)); // The second point is 1/4th of the way in
            cutPoints.Add(points[i] + (v / 2)); // The 3rd is 1/2 of the way in
            cutPoints.Add(points[i] + ((v / 4) * 3)); // The third is 3/4ths
            cutPoints.Add(points[i] + v); // and the last is the same as before

            // Next we do the math for the desired points

            // We then change V into the distance between the intended first and last points
            v = desiredPoints[i + 1] - desiredPoints[i];
            v = v / 3; // Then we cut it since we need 3rds

            // The math is the same as the 8.5 example
            Vector2 a = desiredPoints[i];
            Vector2 b = v + a;
            Vector2 c = Rotate(v, 60) + b;
            Vector2 d = (v * 2f) + a;
            Vector2 e = desiredPoints[i + 1];

            // We then add it in order
            newPoints.Add(a);
            newPoints.Add(b);
            newPoints.Add(c);
            newPoints.Add(d);
            newPoints.Add(e);
        }

        desiredPoints = newPoints; // Then we update both sets of points
        points = cutPoints;
    }

    private GameObject line(List<Vector2> linePoints)
    {
        if (linePoints.Count < 2) // We need 2 points to make a line
        {
            return null;
        }

        GameObject obj = new GameObject();
        obj.name = "Line";
        LineRenderer line = obj.AddComponent<LineRenderer>();

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = line.endColor = Color.black;

        line.startWidth = line.endWidth = 0.1f;

        line.positionCount = linePoints.Count;

        int index = 0;
        foreach (Vector2 point in linePoints)
        {
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