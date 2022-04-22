using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig6 : MonoBehaviour
{
    // A Mover and an Attractor
    Mover2_6 m; 
    Attractor a;

    // Start is called before the first frame update
    void Start()
    {
        m = new Mover2_6();
        a = new Attractor();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Apply the attraction from the Attractor on the Mover
        Vector2 force = a.Attract(m.body); 
        m.ApplyForce(force);
        m.CalculatePosition();
        a.CalculatePosition();
    }
}



public class Attractor
{
    // The properties of an attractor
    private float mass;
    private float G;

    private Vector3 location;
    
    private Rigidbody body;
    private GameObject attractor;

    public Attractor()
    {
        // Create the primitive object
        attractor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        attractor.GetComponent<SphereCollider>().enabled = false;

        // Add a rigidbody to the primitive and set body to reference the component
        attractor.AddComponent<Rigidbody>();
        body = attractor.GetComponent<Rigidbody>();
        body.useGravity = false;

        // Add a material to the object
        Renderer renderer = attractor.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;

        // Set mass and G
        body.mass = 20f;
        G = 9.8f;
    }

    public Vector3 Attract(Rigidbody m)
    {
        Vector3 force = body.position - m.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = G * (body.mass * m.mass) / (distance * distance);
        force *= strength;
        return force;
    }

    public void CalculatePosition()
    {
        attractor.transform.position = location;
    }
}



public class Mover2_6
{
    // The basic properties of a mover class
    public Rigidbody body;
    private Transform transform;

    private Vector2 maximumPos;

    private GameObject mover;

    public Mover2_6()
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        transform = mover.transform;

        mover.AddComponent<Rigidbody>();
        body = mover.GetComponent<Rigidbody>();
        body.useGravity = false;

        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.white;

        body.mass = 10;
        transform.position = new Vector2(5, 0); // Default location
        FindWindowLimits();
    }

    public void ApplyForce(Vector2 force)
    {
        body.AddForce(force, ForceMode.Force);
    }

    public void CalculatePosition()
    {
        CheckEdges();
    }

    private void CheckEdges()
    {
        Vector2 velocity = body.velocity;
        if (transform.position.x > maximumPos.x || transform.position.x < -maximumPos.x)
        {
            velocity.x *= -1;
        }
        if (transform.position.y > maximumPos.y || transform.position.y < -maximumPos.y)
        {
            velocity.y *= -1;
        }
        body.velocity = velocity;
    }

    private void FindWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}