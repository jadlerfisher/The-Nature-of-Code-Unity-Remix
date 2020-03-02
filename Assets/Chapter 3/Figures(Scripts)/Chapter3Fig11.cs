using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig11 : MonoBehaviour
{

    float k = 0.1f;
    float restLength;

    public GameObject bob;
    public GameObject anchor;

    float springLength = 5f;

    Vector3 acceleration = new Vector3(0f, 0f, 0f);
    float mass = 10f;

    //Create variables for rendering the line between two vectors
    private GameObject lineDrawing;
    private LineRenderer lineRender;

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

        connect(bob);
        lineRender.SetPosition(0, anchor.transform.position);
        lineRender.SetPosition(1, bob.transform.position);
    }

    public void connect (GameObject bob)
    {
        Vector3 force = bob.transform.position - anchor.transform.position;
        float d = force.magnitude;
        float stretch = d - springLength;

        Vector3.Normalize(force);
        force *= (-1 * k * stretch);
        applyForce(force);


    }


    //Newton's second law
    //Receive a force, divide by mass, and add to acceleration
    public void applyForce(Vector3 force)
    {
        Vector3 f = force / mass;
        acceleration = acceleration + f;
        bob.transform.position += acceleration;
    }
}
