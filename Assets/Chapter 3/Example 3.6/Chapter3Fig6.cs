using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig6 : MonoBehaviour
{
    [SerializeField] float amplitude = 5f;
    [SerializeField] float angle = 0f;
    [SerializeField] float aVelocity = 0.05f;

    //Create variables for rendering the line between two vectors
    GameObject lineDrawing;
    LineRenderer lineRender;
    GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        // Set the camera to orthographic with a size of 5
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 5;

        // Create a GameObject that will be the line
        lineDrawing = new GameObject();

        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
        lineRender.material = new Material(Shader.Find("Diffuse"));

        //Make the line smaller for aesthetics
        lineRender.startWidth = 0.1f;
        lineRender.endWidth = 0.1f;

        //Begin rendering the line between the two objects. Set the first point (0) at the center Position
        //Make sure the end of the line (1) appears at the new Vector3 in FixedUpdate()
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
