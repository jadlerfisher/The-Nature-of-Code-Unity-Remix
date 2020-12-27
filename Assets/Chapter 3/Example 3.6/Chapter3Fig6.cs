using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig6 : MonoBehaviour
{
    public float amplitude = 100f;
    public float angle = 0f;
    public float aVelocity = 0.05f;

    //Create variables for rendering the line between two vectors
    GameObject lineDrawing;
    LineRenderer lineRender;
    GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        // Create a GameObject that will be the line
        lineDrawing = new GameObject();
        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Diffuse"));
        //Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        //Make sure the end of the line (1) appears at the new Vector3
        Vector2 center = new Vector2(0f, 0f);
        lineRender.SetPosition(0, center);

        //Create the sphere at the end of the line
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //We need to create a new material for WebGL
        Renderer r = sphere.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = amplitude * Mathf.Cos(angle);
        //Using the concept of angular velocity to increment an angle variable
        //Admittedly, in this example we are not really using this variable as an angle, but we will next
        angle += aVelocity;
        
        //Place the sphere and the line at the position
        sphere.transform.position = new Vector2(x, 0f);
        lineRender.SetPosition(1, sphere.transform.position);
    }
}
