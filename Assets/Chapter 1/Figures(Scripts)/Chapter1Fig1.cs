using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig1 : MonoBehaviour
{
    // Variables for the location and speed of mover
    private float x = 0F;
    private float y = 0F;
    private float xSpeed = 0.1F;
    private float ySpeed = 0.1F;

    // Variables to limit the mover within the screen space
    private float xMin, yMin, xMax, yMax;

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
        xMin = minimumPosition.x;
        xMax = maximumPosition.x;
        yMin = minimumPosition.y;
        yMax = maximumPosition.y;

        // We now can set the mover as a primitive sphere in unity
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        // First, we want to update our x and y positions of the mover
        x = mover.transform.position.x;
        y = mover.transform.position.y;

        // Each frame, we will check to see if the mover has touched a boarder
        // We check if the X/Y position is greater than the max position OR if it's less than the minimum position
        bool xHitBoarder = x > xMax || x < xMin;
        bool yHitBoarder = y > yMax || y < yMin;

        // If the mover has hit at all, we will mirror it's speed with the corrisponding boarder

        if (xHitBoarder) {
            xSpeed = -xSpeed;
        }

        if (yHitBoarder) {
            ySpeed = -ySpeed;
        }

        // After we do the checks, we then move the mover by it's x and y speed.
        mover.transform.Translate(xSpeed, ySpeed, 0);
    }
}