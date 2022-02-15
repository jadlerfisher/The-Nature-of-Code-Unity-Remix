using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig10 : MonoBehaviour
{
    // Declare a mover
    Mover1_10 mover;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the mover
        mover = new Mover1_10();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mover.subtractVectors(mousePos, mover.location);
        mover.acceleration = mover.multiplyVector(dir.normalized, .5f);
        mover.Step();
        mover.CheckEdges();
    }
}

public class Mover1_10
{
    // The basic properties of a mover class
    public Vector2 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    private Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover1_10()
    {
        FindWindowLimits();

        // Vector2.zero is a (0, 0) vector
        location = Vector2.zero; 
        velocity = Vector2.zero;
        acceleration = new Vector2(-0.1F, -1F);

        // Set top speed to 2f
        topSpeed = 2f;

        // We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Step()
    {
        if (velocity.magnitude <= topSpeed)
        {
            // Speeds up the mover
            velocity += acceleration * Time.deltaTime; 

            // Limit Velocity to the top speed
            velocity = Vector2.ClampMagnitude(velocity, topSpeed);

            // Moves the mover
            location += velocity * Time.deltaTime;

            // Updates the GameObject position
            mover.transform.position = new Vector3(location.x, location.y, 0);
        }
        else
        {
            velocity -= acceleration * Time.deltaTime;
            location += velocity * Time.deltaTime;
            mover.transform.position = new Vector3(location.x, location.y, 0);
        }
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x = -maximumPos.x;
            ResetMovementVariables();
        }
        else if (location.x < -maximumPos.x)
        {
            location.x = maximumPos.x;
            ResetMovementVariables();
        }
        if (location.y > maximumPos.y)
        {
            location.y = -maximumPos.y;
            ResetMovementVariables();
        }
        else if (location.y < -maximumPos.y)
        {
            location.y = maximumPos.y;
            ResetMovementVariables();
        }
    }

    private void ResetMovementVariables()
    {
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0 for x and y. We will use -10 for z in this example
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }


    // This method calculates A - B component wise
    // subtractVectors(vecA, vecB) will yield the same output as Unity's built in operator: vecA - vecB
    public Vector2 subtractVectors(Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x - vectorB.x;
        float newY = vectorA.y - vectorB.y;
        return new Vector2(newX, newY);
    }

    // This method calculates A * b component wise
    // multiplyVector(vector, factor) will yield the same output as Unity's built in operator: vector * factor
    public Vector2 multiplyVector(Vector2 toMultiply, float scaleFactor)
    {
        float x = toMultiply.x * scaleFactor;
        float y = toMultiply.y * scaleFactor;
        return new Vector2(x, y);
    }

}
