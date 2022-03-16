using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig2 : MonoBehaviour
{
    // A list of multiple Movers
    List<Mover3_2> movers = new List<Mover3_2>(); 
    Attractor2_7 a;

    // Start is called before the first frame update
    void Start()
    {
        int numberOfMovers = 10;
        for (int i = 0; i < numberOfMovers; i++)
        {
            //Each Mover is initialized with a random location, velocity and mass.
            Vector2 randomLocation = new Vector2(Random.Range(-7, 7f), Random.Range(-7f, 7f));
            Vector2 randomVelocity = new Vector2(Random.Range(0f, 5f), Random.Range(0f, 5f));
            float randomMass = Random.Range(.4f, 1f);
            Mover3_2 m = new Mover3_2(randomMass, randomVelocity, randomLocation); 
            movers.Add(m);
        }
        a = new Attractor2_7();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Mover3_2 m in movers)
        {
            Rigidbody body = m.body;

            // Apply the attraction from the Attractor on each Mover object
            Vector2 force = a.Attract(body); 
            m.body.AddForce(force, ForceMode.Force);
            // Apply rotation to the mover based on the currently applied force
            m.ApplyRotation(force);
            m.CheckEdges();
        }
    }
}

public class Mover3_2
{
    public Rigidbody body;
    public Transform transform;
    private GameObject gameObject;

    private Vector2 maximumPos;

    private Vector3 aAcceleration;

    public Mover3_2(float randomMass, Vector2 initialVelocity, Vector2 initialPosition)
    {
        // Create a primitive cube for each mover and destroy the box collider to avoid collisions
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject.Destroy(gameObject.GetComponent<BoxCollider>());
        transform = gameObject.transform;

        // Instantiate a rigidbody for the mover to accept forces
        gameObject.AddComponent<Rigidbody>();
        body = gameObject.GetComponent<Rigidbody>();
        body.useGravity = false;

        // Instantiate a renderer and provide a shader
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.white;

        // Modify the scale of the object to give correlation between size and mass
        gameObject.transform.localScale = new Vector3(randomMass, randomMass, randomMass);
        body.mass = randomMass;

        body.position = initialPosition; // Random location passed into the constructor
        body.velocity = initialVelocity; // The extra velocity makes the mover orbit

        // Instantiate the window limits
        FindWindowLimits();
    }

    // Pass in the applied force and calculate a rotation based on the application of force
    public void ApplyRotation(Vector3 angularForce)
    {
        aAcceleration.x = angularForce.x / 2f;
        aAcceleration.y = angularForce.y / 2f;
        aAcceleration.z = angularForce.z / 2f;

        transform.Rotate(aAcceleration);
    }

    //Checks to ensure the body stays within the boundaries
    public void CheckEdges()
    {
        Vector2 velocity = body.velocity;
        if (body.position.x > maximumPos.x || body.position.x < -maximumPos.x)
        {
            velocity.x *= -1 * Time.deltaTime; ;
        }
        if (body.position.y > maximumPos.y || body.position.y < -maximumPos.y)
        {
            velocity.y *= -1 * Time.deltaTime; ;
        }
        body.velocity = velocity;
    }

    // Find the edges of the screen
    private void FindWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}

