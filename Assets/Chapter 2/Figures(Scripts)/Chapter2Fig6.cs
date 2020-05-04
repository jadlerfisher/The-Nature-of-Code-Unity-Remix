using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig6 : MonoBehaviour
{
    Mover m; // A Mover and an Attractor
    Attractor a;

    // Start is called before the first frame update
    void Start()
    {
        m = new Mover();
        a = new Attractor();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 force = a.Attract(m); // Apply the attraction from the Attractor on the Mover
        m.ApplyForce(force);
        m.Update();
        a.Update();
    }
}



public class Attractor
{
    public float mass;
    private Vector2 location;
    private float G;

    private GameObject attractor;

    public Attractor()
    {
        attractor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer renderer = attractor.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;

        location = Vector2.zero;
        mass = 20f;
        G = 9.8f;
    }

    public Vector2 Attract(Mover m)
    {
        Vector2 force = location - (Vector2)m.transform.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (G * mass * m.rigidbody.mass) / (distance * distance);
        force *= strength;
        return force;
    }

    public void Update()
    {
        attractor.transform.position = location;
    }
}



public class Mover
{
    // The basic properties of a mover class
    public Transform transform;
    public Rigidbody rigidbody;

    private Vector2 minimumPos, maximumPos;

    private GameObject mover;

    public Mover()
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        transform = mover.transform;
        mover.AddComponent<Rigidbody>();
        rigidbody = mover.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.white;

        rigidbody.mass = 1;
        transform.position = new Vector2(5, 0); // Default location
        rigidbody.velocity = new Vector2(0, -4); // The extra velocity makes the mover orbit
        findWindowLimits();
    }

    public void ApplyForce(Vector2 force)
    {
        rigidbody.AddForce(force);
    }

    public void Update()
    {
        CheckEdges();
    }

    public void CheckEdges()
    {
        Vector2 velocity = rigidbody.velocity;
        if (transform.position.x > maximumPos.x || transform.position.x < minimumPos.x)
        {
            velocity.x *= -1;
        }
        if (transform.position.y > maximumPos.y || transform.position.y < minimumPos.y)
        {
            velocity.y *= -1;
        }
        rigidbody.velocity = velocity;
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 8;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}