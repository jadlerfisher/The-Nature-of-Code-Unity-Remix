using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig7 : MonoBehaviour
{
    List<Mover2_7> movers = new List<Mover2_7>(); // Now we have multiple Movers!
    Attractor2_7 a;

    // Start is called before the first frame update
    void Start()
    {
        int numberOfMovers = 10;
        for (int i = 0; i < numberOfMovers; i++)
        {
            Vector2 randomLocation = new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f));
            Vector2 randomVelocity = new Vector2(Random.Range(0f, 5f), Random.Range(0f, 5f));
            Mover2_7 m = new Mover2_7(Random.Range(0.2f, 1f), randomVelocity, randomLocation); //Each Mover is initialized randomly.
            movers.Add(m);
        }
        a = new Attractor2_7();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Mover2_7 m in movers)
        {
            Vector2 force = a.Attract(m); // Apply the attraction from the Attractor on each Mover object

            m.ApplyForce(force);
            m.Update();
        }
    }
}


public class Attractor2_7
{
    public float mass;
    private Vector2 location;
    private float G;

    private GameObject attractor;

    public Attractor2_7()
    {
        attractor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject.Destroy(attractor.GetComponent<SphereCollider>());
        Renderer renderer = attractor.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;

        location = Vector2.zero;
        mass = 20f;
        G = 9.8f;
        attractor.transform.position = location;
    }

    public Vector2 Attract(Mover2_7 m)
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
}

public class Mover2_7
{
    // The basic properties of a mover class
    public Transform transform;
    public Rigidbody rigidbody;

    private Vector2 minimumPos, maximumPos;

    private GameObject mover;

    public Mover2_7(float randomMass, Vector2 initialVelocity, Vector2 initialPosition)
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject.Destroy(mover.GetComponent<SphereCollider>());
        transform = mover.transform;
        mover.AddComponent<Rigidbody>();
        rigidbody = mover.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Transparent/Diffuse"));
        renderer.material.color = Color.white;
        mover.transform.localScale = new Vector3(randomMass, randomMass, randomMass);

        rigidbody.mass = 1;
        transform.position = initialPosition; // Default location
        rigidbody.velocity = initialVelocity; // The extra velocity makes the mover orbit
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
        Camera.main.orthographicSize = 10;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}