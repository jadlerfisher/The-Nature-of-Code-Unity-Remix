using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter8Fig5Animated : MonoBehaviour
{
    Vector2 maximumPos;
    List<Vector2> points;
    List<Vector2> desiredPoints;
    GameObject KochLine;
    WaitForSeconds pause;

    int calls = 0;

    // Start is called before the first frame update
    void Start()
    {
        FindWindowLimits();

        // We need a desired set to move towards in the curve
        points = new List<Vector2>();
        desiredPoints = new List<Vector2>(); 

        Vector2 a = new Vector2(-maximumPos.x, -maximumPos.y + 1f);
        Vector2 b = new Vector2(maximumPos.x, -maximumPos.y + 1f);

        desiredPoints.Add(a);
        desiredPoints.Add(b);

        // for a nicer starting effect, we'll start the line at 0,0
        points.Add(Vector2.zero); 
        points.Add(Vector2.zero);

        // Define the WaitForSeconds()
        pause = new WaitForSeconds(5f);

        // We can then start a Coroutine to do the generations over time
        StartCoroutine(Iterate()); 
    }

    private IEnumerator Iterate() 
    {
        // We have to limit the number of times we call generate() otherwise the LineRenderer will reach max points
        while (calls < 6) 
        {
            // After 5 seconds, the function will continue
            yield return pause; 
            Generate();
            calls++;
        }
    }

    private void Update()
    {
        if (KochLine != null) 
        {
            // If the KochLine exsists, we're goint to replace it
            Destroy(KochLine); 
        }

        for (int i = 0; i < desiredPoints.Count; i++) 
        {
            Vector2 desired = desiredPoints[i];
            Vector2 at = points[i];

            // For each point, we move the current point towards the desitred point by 30% every second.
            // Time.deltaTime will help convert the 'Per Frame' speed of the Update Function to a 'Per Second' one
            points[i] += ((desired - at) / 1.3f) * Time.deltaTime; 
        }

        KochLine = Line(points); // We then regenerate the line
    }

    void Generate()
    {
        List<Vector2> newPoints = new List<Vector2>();
        List<Vector2> cutPoints = new List<Vector2>();

        // We're going to use 2 points each time to make a line
        for (int i = 0; i < desiredPoints.Count - 1; i++) 
        {
            // First we make sure to add the points to the real line so they can be animated
            Vector2 v = points[i + 1] - points[i];

            cutPoints.Add(points[i]); // We keep the first point
            cutPoints.Add(points[i] + ((v / 4) * 1)); // The second point is 1/4th of the way in
            cutPoints.Add(points[i] + (v / 2)); // The 3rd is 1/2 of the way in
            cutPoints.Add(points[i] + ((v / 4) * 3)); // The third is 3/4ths
            cutPoints.Add(points[i] + v); // and the last is the same as before

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

        // Then we update both sets of points
        desiredPoints = newPoints; 
        points = cutPoints;
    }

    private GameObject Line(List<Vector2> linePoints)
    {
        // We need 2 points to make a line
        if (linePoints.Count < 2) 
        {
            return null;
        }

        GameObject obj = new GameObject();
        obj.name = "Line";
        LineRenderer line = obj.AddComponent<LineRenderer>();

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = line.endColor = Color.black;
        line.widthMultiplier = 0.1f;

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

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -5);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}