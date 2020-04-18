using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig7 : MonoBehaviour
{
    // Variables for the location and speed of mover
    private Vector2 location = new Vector2(0F, 0F);
    private Vector2 velocity = new Vector2(0.1F, 0.1F);

    // Variables to limit the mover within the screen space
    private Vector2 minimumPos, maximumPos;

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
        minimumPos = new Vector2(minimumPosition.x, minimumPosition.y);
        maximumPos = new Vector2(maximumPosition.x, maximumPosition.y);

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

        // Lets now update the location of the mover
        location += velocity;

        // Now we apply the positions to the mover to put it in it's place
        mover.transform.position = new Vector3(location.x, location.y, 0);
    }
}








