using UnityEngine;

public class IntroductionFig1 : MonoBehaviour
{
    //We need to create a walker
    introMover walker;

    // Start is called before the first frame update
    void Start()
    {
        walker = new introMover();
    }

    // Update is called once per frame
    void FixedUpdate()
    {        //Have the walker choose a direction
        walker.step();
        walker.CheckEdges();
    }
}


public class introMover
{
    // The basic properties of a mover class
    // x, y, z 
    private Vector3 location; 

    // The window limits
    private Vector2 minimumPos, maximumPos;

    // Gives the class a GameObject to draw on the screen
    public GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public introMover()
    {
        findWindowLimits();
        location = Vector2.zero;
        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void step()
    {
        location = mover.transform.position;
        //Each frame choose a new Random number 0,1,2,3, 
        //If the number is equal to one of those values, take a step
        int choice = Random.Range(0, 4);
        if (choice == 0)
        {
            location.x++;

        }
        else if (choice == 1)
        {
            location.x--;
        }
        else if (choice == 3)
        {
            location.y++;
        }
        else
        {
            location.y--;
        }

        mover.transform.position += location * Time.deltaTime;
    }

    public void CheckEdges()
    {
        location = mover.transform.position;

        if (location.x > maximumPos.x)
        {
            location = Vector2.zero;
        }
        else if (location.x < minimumPos.x)
        {
            location = Vector2.zero;
        }
        if (location.y > maximumPos.y)
        {
            location = Vector2.zero;
        }
        else if (location.y < minimumPos.y)
        {
            location = Vector2.zero;
        }
        mover.transform.position = location;
    }

    private void findWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}

