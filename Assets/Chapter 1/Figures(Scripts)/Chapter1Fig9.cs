using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig9 : MonoBehaviour
{
    // Declare a mover object
    private Mover1_9 mover;

    // Start is called before the first frame update
    void Start()
    {
        // Create a Mover object
        mover = new Mover1_9();
    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        mover.Update();
        mover.CheckEdges();
    }
}

public class Mover1_9
{
    // The basic properties of a mover class
    private Vector2 location, velocity, acceleration;
    private float topSpeed;

    // The window limits
    private Vector2 minimumPos, maximumPos;


    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover1_9()
    {
        findWindowLimits();
        location = Vector2.zero; // Vector2.zero is a (0, 0) vector
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        topSpeed = 2F;

        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Update()
    {
        // Random acceleration but it's not normalized!
        acceleration = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        // Normilize the acceletation
        acceleration.Normalize();
        // Now we can scale the magnitude as we wish!
        acceleration *= Random.Range(5f, 10f);

        // Speeds up the mover
        velocity += acceleration * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

        // Limit Velocity to the top speed
        velocity = Vector2.ClampMagnitude(velocity, topSpeed);

        // Moves the mover
        location += velocity * Time.deltaTime;


        // Updates the GameObject of this movement
        mover.transform.position = new Vector3(location.x, location.y, 0);
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x -= maximumPos.x - minimumPos.x;
        }
        else if (location.x < minimumPos.x)
        {
            location.x += maximumPos.x - minimumPos.x;
        }
        if (location.y > maximumPos.y)
        {
            location.y -= maximumPos.y - minimumPos.y;
        }
        else if (location.y < minimumPos.y)
        {
            location.y += maximumPos.y - minimumPos.y;
        }
    }

    private void findWindowLimits()
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




