using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig6 : MonoBehaviour
{
    Mover2_6 m; // A Mover and an Attractor
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
        Vector2 force = a.Attract(m.body); // Apply the attraction from the Attractor on the Mover
        m.ApplyForce(force);
        m.Update();
        a.Update();
    }
}



public class Attractor
{
    public float mass;
    private Vector3 location;
    private float G;
    public Rigidbody body;

    private GameObject attractor;

    public Attractor()
    {
        attractor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        attractor.GetComponent<SphereCollider>().enabled = false;

        attractor.AddComponent<Rigidbody>();
        body = attractor.GetComponent<Rigidbody>();
        body.useGravity = false;
        Renderer renderer = attractor.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;

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

    public void Update()
    {
        attractor.transform.position = location;
    }
}



public class Mover2_6
{
    // The basic properties of a mover class
    public Transform transform;
    public Rigidbody body;

    private Vector2 minimumPos, maximumPos;

    private GameObject mover;

    public Mover2_6()
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        transform = mover.transform;
        mover.AddComponent<Rigidbody>();
        body = mover.GetComponent<Rigidbody>();
        body.useGravity = true;
        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.white;

        body.mass = 10;
        transform.position = new Vector2(5, 0); // Default location
        //body.velocity = new Vector2(0, -4); // The extra velocity makes the mover orbit
        findWindowLimits();
    }

    public void ApplyForce(Vector2 force)
    {
        body.AddForce(force, ForceMode.Force);
    }

    public void Update()
    {
        CheckEdges();
    }

    public void CheckEdges()
    {
        Vector2 velocity = body.velocity;
        if (transform.position.x > maximumPos.x || transform.position.x < minimumPos.x)
        {
            velocity.x *= -1;
        }
        if (transform.position.y > maximumPos.y || transform.position.y < minimumPos.y)
        {
            velocity.y *= -1;
        }
        body.velocity = velocity;
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 4;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}