using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig7 : MonoBehaviour
{
    List<Mover> movers = new List<Mover>(); // Now we have multiple Movers!
    Attractor a;

    // Start is called before the first frame update
    void Start()
    {
        int numberOfMovers = 10;
        for (int i = 0; i < numberOfMovers; i++) {
            Vector2 randomLocation = new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f));
            Mover m = new Mover(Random.Range(0.2f, 2f), Random.Range(1f,5f), randomLocation); //Each Mover is initialized randomly.
            movers.Add(m);
        }
        a = new Attractor();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Mover m in movers) {
            Vector2 force = a.Attract(m); // Apply the attraction from the Attractor on each Mover object

            Debug.Log(force);

            m.ApplyForce(force);
            m.Update();
        }
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
        attractor.transform.position = location;
    }

    public Vector2 Attract(Mover m)
    {
        Vector2 force = location - m.location;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (G * mass * m.mass) / (distance * distance);
        force *= strength;
        return force;
    }
}

public class Mover
{
    // The basic properties of a mover class
    public Vector2 location, velocity, acceleration;
    public float mass;

    private Vector2 minimumPos, maximumPos;

    private GameObject mover;

    public Mover(float randomMass, float initialVelocity, Vector2 initialPosition)
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Transparent/Diffuse"));
        renderer.material.color = Color.white;
        mover.transform.localScale = new Vector3(randomMass, randomMass, randomMass);

        mass = randomMass;
        location = initialPosition;
        velocity = new Vector2(0, initialVelocity);
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
        Camera.main.orthographicSize = 10;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}