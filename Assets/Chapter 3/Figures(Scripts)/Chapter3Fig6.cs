using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig6 : MonoBehaviour
{
    float period = 22f;
    float amplitude = 20f;

    float angle = 0f;
    float aVelocity = 0.05f;

    public GameObject sphere;

    //Create variables for rendering the line between two vectors
    private GameObject lineDrawing;
    private LineRenderer lineRender;

    Vector3 force;
    float mass;
    Vector3 acceleration;

    // Start is called before the first frame update
    void Start()
    {
        // Create a GameObject that will be the line
        lineDrawing = new GameObject();

        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing. We will see a bright pink line.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        float x = amplitude * Mathf.Cos(angle);
        angle += aVelocity;
            
            //Mathf.Cos((2 * Mathf.PI) * Time.time / period);

        sphere.transform.position = new Vector3(x, 0f, 0f);

        //Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        //Make sure the end of the line (1) appears at the new Vector3
        Vector3 center = new Vector3(0f, 0f, 0f);
        lineRender.SetPosition(0, center);
        lineRender.SetPosition(1, sphere.transform.position);
    }

    //Newton's second law
    //Receive a force, divide by mass, and add to acceleration
    public void applyForce(Vector3 force)
    {
        Vector3 f = force / mass;
        acceleration = acceleration + f;
    }
}
