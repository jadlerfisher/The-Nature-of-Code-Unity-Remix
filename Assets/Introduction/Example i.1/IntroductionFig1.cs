using UnityEngine;

public class IntroductionFig1 : MonoBehaviour
{
    // We need to create a mover instance
    IntroMover mover;

    // Start is called before the first frame update
    void Start()
    {
        mover = new IntroMover();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Have the mover step and check edges
        mover.Step();
        mover.CheckEdges();
    }
}


public class IntroMover
{
    // The basic properties of a mover class
    // x, y, z 
    private Vector3 location; 

    // The window limits
    private Vector2 maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public IntroMover()
    {
        FindWindowLimits();
        // Set location to Vector2.zero, shorthand for (0, 0)
        location = Vector2.zero;
        // We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void Step()
    {
        location = mover.transform.position;
        // Each frame choose a new Random number 0,1,2,3
        // If the number is equal to one of those values, take a step
        // Random.Range() is MaxExclusive while using integer values, possible values 0,1,2,3
        int choice = Random.Range(0, 4);
        if (choice == 0)
        {
            location.x++;

        }
        else if (choice == 1)
        {
            location.x--;
        }
        else if (choice == 2)
        {
            location.y++;
        }
        else if (choice == 3)
        {
            location.y--;
        }

        mover.transform.position += location * Time.deltaTime;
    }

    public void CheckEdges()
    {
        location = mover.transform.position;
        // Reset location to zero upon reaching maximum or(||) negative maximum(minimum)
        if (location.x > maximumPos.x || location.x < -maximumPos.x)
        {
            location = Vector2.zero;
        }
        if (location.y > maximumPos.y || location.y < -maximumPos.y)
        {
            location = Vector2.zero;
        }
        // Set the position of the mover to location
        mover.transform.position = location;
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Ensure the camera is positioned in the center of the scene
        Camera.main.transform.position = Vector2.zero;
        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        // The maximum position can be attributed to the minimum bounds by setting it negative
        // maximumPos and -maximumPos equate to maximum and minimum screen bounds
    }
}

