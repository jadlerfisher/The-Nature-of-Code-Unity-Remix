using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig7 : MonoBehaviour
{
    // Declare a mover object
    private Mover1_7 mover;

    // Variables to limit the mover within the screen space
    private float xMin, yMin, xMax, yMax;

    // Start is called before the first frame update
    void Start()
    {
        // Create a Mover object
        mover = new Mover1_7();
    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        mover.Step();
        mover.CheckEdges();
    }
}

public class Mover1_7
{
    // The basic properties of a mover class
    private Vector2 location, velocity;

    // The window limits
    private Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover1_7()
    {
        FindWindowLimits();
        location = new Vector2(Random.Range(-maximumPos.x, maximumPos.x), Random.Range(-maximumPos.y, maximumPos.y));
        velocity = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));

        // We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Step()
    {
        // Moves the mover, Time.deltaTime is the time passed since the last frame and ties movement to a fixed rate instead of framerate.
        location += velocity * Time.deltaTime; 

        // Updates the GameObject to the new position
        mover.transform.position = new Vector2(location.x, location.y);
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x = -maximumPos.x;
        }
        else if (location.x < -maximumPos.x)
        {
            location.x = maximumPos.x;
        }
        if (location.y > maximumPos.y)
        {
            location.y = -maximumPos.y;
        }
        else if (location.y < -maximumPos.y)
        {
            location.y = maximumPos.y;
        }
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
}




