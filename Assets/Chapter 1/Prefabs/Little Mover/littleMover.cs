using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleMover : MonoBehaviour
{

    // Create your speed variables for the mover class
    public Vector2 location = Vector2.zero;
    public Vector2 velocity = Vector2.zero;
    public Vector2 acceleration = Vector2.zero;
    public float topSpeed = 1f;

    //Create a variable to access the Mover's information
    private GameObject mover;
    //Float coordinates for our Little Mover

    // The window limits
    private Vector2 minimumPos, maximumPos;

    // Start is called before the first frame update
    void Awake()
    {

        //assign the mover's GameObject to the varaible
        mover = this.gameObject;

        //Set the initial spawn location to 0,0,0
        location = new Vector3(Random.Range(-1F, 1F), Random.Range(-1F, 1F), Random.Range(-1F, 1F));

        //Assign that spawn location to the mover
        mover.transform.position = location;
        findWindowLimits();



    }
    // Update is called once per frame
    void Update()
    {
        CheckEdges();
        if (velocity.magnitude <= topSpeed)
        {
            // Speeds up the mover
            velocity += acceleration * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

            // Limit Velocity to the top speed
            velocity = Vector2.ClampMagnitude(velocity, topSpeed);

            // Moves the mover
            location += velocity * Time.deltaTime;

            // Updates the GameObject of this movement
            mover.transform.position = new Vector3(location.x, location.y, 0);

        } else
        {
            // Vector3 velocity3 = transform.InverseTransformPoint(velocity);
            // velocity = new Vector2(velocity3.x, velocity3.y);
            velocity -= acceleration * Time.deltaTime;
            location += velocity * Time.deltaTime;
            mover.transform.position = new Vector3(location.x, location.y, 0);
        }

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

    void CheckEdges()
    {
        //Each frame, check to see whether the ball's x,y, or z position coordinates have HIT a border and if so, to either add a value (+=) or substract a value (-=) from the vector. 
        //Then we need to bounce off the wall along a particular vector.
        if (location.x > maximumPos.x)
        {
            velocity = Vector2.zero;
            acceleration = Vector2.zero;
            location.x += velocity.x;

            if (location.x > maximumPos.x)
            {
                location.x -= velocity.x + 1f;

            }
        }
        else if (location.x < minimumPos.x)
        {
            velocity = Vector2.zero;
            acceleration = Vector2.zero;
            location.x += velocity.x;

            location.x -= velocity.x;
            if (location.x < minimumPos.x)
            {
                location.x += velocity.x + 1f;

            }
        }
        if (location.y > maximumPos.y)
        {
            velocity = Vector2.zero;
            acceleration = Vector2.zero;
            location.y += velocity.y;

            if (location.y > maximumPos.y)
            {
                location.y -= velocity.y + 1f;

            }
        }
        else if (location.y < minimumPos.y)
        {
            velocity = Vector2.zero;
            acceleration = Vector2.zero;
            location.y -= velocity.y;

            if (location.y < minimumPos.y)
            {
                location.y += velocity.y + 1f;

            }
        }
    }
     void findWindowLimits()
    {
        // The code to find the information on the camera as seen in Figure 1.2

        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // We now find the Width and Height of the camera screen
        float width = Camera.main.pixelWidth;
        float height = Camera.main.pixelHeight;
        // Next we grab the minimum and maximum position for the screen
        Vector3 minimumPosition = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 maximumPosition = Camera.main.ScreenToWorldPoint(new Vector3(width, height, 0));
        // We can now properly assign the Min and Max for out scene
        minimumPos = new Vector2(minimumPosition.x, minimumPosition.y);
        maximumPos = new Vector2(maximumPosition.x, maximumPosition.y);
    }


}
