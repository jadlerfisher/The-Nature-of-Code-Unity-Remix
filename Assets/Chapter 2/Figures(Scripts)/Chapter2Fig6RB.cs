using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig6RB : MonoBehaviour
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
    void Update()
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

    public Vector2 Attract(Mover m) {
        Vector2 force = location - m.location;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (G * mass * m.mass) / (distance * distance);
        force *= strength;
        return force;
    }

    public void Update() {
        attractor.transform.position = location;
    }
}

public class Mover
{
    // The basic properties of a mover class
    public Vector2 location, velocity, acceleration;
    public float mass;

    private Vector2 minimumPos, maximumPos;

    private GameObject mover;

    public Mover() 
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.white;

        mass = 1;
        location = new Vector2(5, 0); // Default location
        velocity = new Vector2(0, -4); // The extra velocity makes the mover orbit
        acceleration = Vector2.zero;
        findWindowLimits();
    }

    public void ApplyForce(Vector2 force) 
    {
        Vector2 f = force / mass;
        acceleration += f;
    }

    public void Update() 
    {
        velocity += acceleration * Time.deltaTime;
        location += velocity * Time.deltaTime;

        acceleration = Vector2.zero;

        mover.transform.position = location;
        CheckEdges();
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x || location.x < minimumPos.x)
        {
            velocity.x *= -1;
        }
        if (location.y > maximumPos.y || location.y < minimumPos.y)
        {
            velocity.y *= -1;
        }
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 8;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}