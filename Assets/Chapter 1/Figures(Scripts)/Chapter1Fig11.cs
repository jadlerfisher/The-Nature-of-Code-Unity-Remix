using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig11 : MonoBehaviour
{

    public List<Mover1_11> Movers = new List<Mover1_11>();
    private int amountMovers = 30;


    // Start is called before the first frame update
    void Start()
    {
        // We need to instantiate our Movers and add them to a list
        while (Movers.Count < 10)
        {

            Movers.Add(new Mover1_11());
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < Movers.Count; i++)
        {
            Vector2 dir = Movers[i].subtractVectors(mousePos, Movers[i].location);
            Movers[i].acceleration = Movers[i].multiplyVector(dir.normalized, .5f);
            Movers[i].Update();
        }
    }

}

public class Mover1_11
{

    // The basic properties of a mover class
    public Vector2 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover1_11()
    {
        findWindowLimits();
        location = new Vector2(Random.Range(-4f, 4f), Random.Range(-4f, 4f)); 
        velocity = Vector2.zero;
        acceleration = Vector2.zero; // Vector2.zero is a (0, 0) vector
        topSpeed = 1;
    }

    public void Update()
    {
        CheckEdges();
        if (velocity.magnitude <= topSpeed)
        {
            // Speeds up the mover
            velocity += acceleration * Time.deltaTime;

            // Limit Velocity to the top speed
            velocity = Vector2.ClampMagnitude(velocity, topSpeed);

            // Moves the mover
            location += velocity * Time.deltaTime;

            // Updates the GameObject of this movement
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
            location.x -= maximumPos.x - minimumPos.x;
            acceleration =  Vector2.zero;
            velocity = Vector2.zero;
        }
        else if (location.x < minimumPos.x)
        {
            location.x += maximumPos.x - minimumPos.x;
            acceleration = Vector2.zero;
            velocity = Vector2.zero;
        }
        if (location.y > maximumPos.y)
        {
            location.y -= maximumPos.y - minimumPos.y;
            acceleration = Vector2.zero;
            velocity = Vector2.zero;
        }
        else if (location.y < minimumPos.y)
        {
            location.y += maximumPos.y - minimumPos.y;
            acceleration = Vector2.zero;
            velocity = Vector2.zero;
        }
    }

    private void findWindowLimits()
    {
        // The code to find the information on the camera as seen in Figure 1.2

        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
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
