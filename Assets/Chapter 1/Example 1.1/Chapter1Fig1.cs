using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

        // Next we grab the minimum and maximum position for the screen
        Vector2 minimumPosition = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Vector2 maximumPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        // We can now properly assign the Min and Max for out scene
        xMin = minimumPosition.x;
        xMax = maximumPosition.x;
        yMin = minimumPosition.y;
        yMax = maximumPosition.y;

        // We now can set the mover as a primitive sphere in unity
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
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

        // Lets now update the location of the mover
        x += xSpeed;
        y += ySpeed;

        // Now we apply the positions to the mover to put it in it's place
        mover.transform.position = new Vector2(x, y);
    }
}