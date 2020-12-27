using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig7 : MonoBehaviour
{
    List<oscillator> oscilattors = new List<oscillator>();

    void Start()
    {
        while (oscilattors.Count < 10)
        {
            oscilattors.Add(new oscillator());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (oscillator o in oscilattors)
        {
            //Each oscillator object oscillating on the x-axis
            float x = Mathf.Sin(o.angle.x) * o.amplitude.x;
            //Each oscillator object oscillating on the y-axis
            float y = Mathf.Sin(o.angle.y) * o.amplitude.y;
            //Add the oscillator's velocity to its angle
            o.angle += o.velocity;
            // Draw the line for each oscillator
            o.lineRender.SetPosition(1, o.oGameObject.transform.position);
            //Move the oscillator
            o.oGameObject.transform.transform.Translate(new Vector2(x, y) * Time.deltaTime);
        }
    }
}

public class oscillator {

    // The basic properties of an oscillator class
    public Vector2 velocity, angle, amplitude;

    // The window limits
    private Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    public GameObject oGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    //Create variables for rendering the line between two vectors
    public LineRenderer lineRender;

    public oscillator()
    {    
        findWindowLimits();
        angle = Vector2.zero;
        velocity = new Vector2(Random.Range(-.05f, .05f), Random.Range(-0.05f, 0.05f));
        amplitude = new Vector2(Random.Range(-maximumPos.x/2, maximumPos.x/2), Random.Range(-maximumPos.y/2, maximumPos.y/2));

        //We need to create a new material for WebGL
        Renderer r = oGameObject.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        
        // Create a GameObject that will be the line
        GameObject lineDrawing = new GameObject();
        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Diffuse"));
        //Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        //Make sure the end of the line (1) appears at the new Vector3
        Vector2 center = new Vector2(0f, 0f);
        lineRender.SetPosition(0, center);
    }

    private void findWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}