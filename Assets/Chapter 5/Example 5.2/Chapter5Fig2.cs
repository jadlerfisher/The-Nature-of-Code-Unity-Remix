using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter5Fig2 : MonoBehaviour
{
    // Declare a mover object
    private Mover5_2 mover;
    //Create the Raycast Mover
    private rayCastMover raymover;

    // Start is called before the first frame update
    void Start()
    {
        // Create a Mover object
        mover = new Mover5_2();
        // Raycast Mover
        raymover = new rayCastMover();
    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        mover.Update();
        mover.CheckEdges();
        raymover.lookForFood();
    }
}


public class rayCastMover{

    private Vector2 location, velocity;
    // Gives the class a GameObject to draw on the screen
    private GameObject rayMover = GameObject.CreatePrimitive(PrimitiveType.Cube);

    // We are going to rotate on the z-axis. We'll rename this as a velocity;
    public Vector3 aVelocity = new Vector3(0f, 0f, 0f);
    public Vector3 aAcceleration = new Vector3(0f, 0f, .001f);

    public Rigidbody body;

    //Debug
    private LineRenderer lineRender;

    public rayCastMover() {

        rayMover.transform.position = new Vector2(3f, -2f);

        Renderer renderer = rayMover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;
        //Add a rigidybody so we can work use physics
        body = rayMover.AddComponent<Rigidbody>();
        body.useGravity = false;
        //This locks the RigidBody so that it does not move or rotate in the Z axis.
        body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;

        // Add the Unity Component "LineRenderer" to the GameObject this script is attached to
        lineRender = rayMover.AddComponent<LineRenderer>();
        //We need to create a new material for WebGL
        lineRender.material = new Material(Shader.Find("Diffuse"));
        //We all need to make the lineRenderer as narrow as our Raycast
        //Which is quite small
        lineRender.widthMultiplier = .01f;

    }

    public void seePrey()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(rayMover.transform.position, rayMover.transform.up, out hit, Mathf.Infinity))
        {
            lineRender.material.color = Color.white;
            lineRender.SetPosition(0, rayMover.transform.position);
            lineRender.SetPosition(1, hit.transform.position);

            Vector2 attackVector = attack(hit.transform.gameObject.GetComponent<Rigidbody>());
            body.AddForce(attackVector, ForceMode.Impulse);
        }
        else
        {
            lineRender.SetPosition(0, rayMover.transform.position);
            lineRender.SetPosition(1, rayMover.transform.up * 1000);
        }
    }

    //This is a modification of our Attract method from Chapter 2
    public Vector2 attack(Rigidbody m)
    {
        Vector2 force = body.position - m.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (-9.81f * body.mass * m.mass) / (distance * distance);
        force *= strength;
        return force;
    }

    public void lookForFood()
    {
        aVelocity += aAcceleration;
        rayMover.transform.Rotate(aVelocity, Space.World);
        seePrey();
    }
}


public class Mover5_2
{
    // The basic properties of a mover class
    private Vector2 location, velocity;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    public Rigidbody body;

    public Mover5_2()
    {
        findWindowLimits();
        location = Vector2.zero;
        velocity = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        body = mover.AddComponent<Rigidbody>();
        body.useGravity = false;
        //This locks the RigidBody so that it does not move or rotate in the Z axis.
        body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Update()
    {
        velocity = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
        // Moves the mover
        location += velocity * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.
        // Updates the GameObject of this movement
        mover.transform.position = new Vector2(location.x, location.y);
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x -= maximumPos.x - minimumPos.x;
        }
        else if (location.x < minimumPos.x)
        {
            location.x += maximumPos.x - minimumPos.x;
        }
        if (location.y > maximumPos.y)
        {
            location.y -= maximumPos.y - minimumPos.y;
        }
        else if (location.y < minimumPos.y)
        {
            location.y += maximumPos.y - minimumPos.y;
        }
    }

    private void findWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}