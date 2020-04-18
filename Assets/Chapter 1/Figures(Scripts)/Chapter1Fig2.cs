using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Now that we understand how to add two vectors together, we can look at how addition is implemented in the PVector class itself. Let’s write a function called add() that takes another PVector object as its argument.

public class PVector
{

    public float x;
    public float y;

    public PVector(float x_, float y_)
    {
        x = x_;
        y = y_;
    }

    public void add(PVector v)
    {
        x = x + v.x;
        y = y + v.y;
    }

}



public class Chapter1Fig2 : MonoBehaviour
{
    // Variables for the location and speed of mover
    private PVector location = new PVector(0F, 0F);
    private PVector velocity = new PVector(0.1F, 0.1F);

    // Variables to limit the mover within the screen space
    private PVector minimumPos, maximumPos;

    // A Variable to represent our mover in the scene
    private GameObject mover;

    // Start is called before the first frame update
    void Start()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // We now find the Width and Height of the camera screen
        float width = Camera.main.pixelWidth;
        float height = Camera.main.pixelHeight;

        // Next we grab the minimum and maximum position for the screen
        Vector3 minimumPosition = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 maximumPosition = Camera.main.ScreenToWorldPoint(new Vector3(width, height, 0));

        // We can now properly assign the Min and Max for out scene
        minimumPos = new PVector(minimumPosition.x, minimumPosition.y);
        maximumPos = new PVector(maximumPosition.x, maximumPosition.y);

        // We now can set the mover as a primitive sphere in unity
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        // Each frame, we will check to see if the mover has touched a boarder
        // We check if the X/Y position is greater than the max position OR if it's less than the minimum position
        bool xHitBoarder = location.x > maximumPos.x || location.x < minimumPos.x;
        bool yHitBoarder = location.y > maximumPos.y || location.y < minimumPos.y;

        // If the mover has hit at all, we will mirror it's speed with the corrisponding boarder

        if (xHitBoarder)
        {
            velocity.x = -velocity.x;
        }

        if (yHitBoarder)
        {
            velocity.y = -velocity.y;
        }

        // Lets now add the velocity to our location to update our position
        location.add(velocity);

        // Now we apply the positions to the mover to put it in it's place
        mover.transform.position = new Vector3(location.x, location.y, 0);
    }
}

