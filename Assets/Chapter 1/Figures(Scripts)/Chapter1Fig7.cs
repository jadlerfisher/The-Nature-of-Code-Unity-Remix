﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig7 : MonoBehaviour
{
    // Declare a mover object
    private Mover mover;

    // Start is called before the first frame update
    void Start()
    {
        // Create a Mover object
        mover = new Mover();
    }

    // Update is called once per frame forever and ever (until you quit).
    void Update()
    {
        mover.Update();
        mover.CheckEdges();
    }
}

public class Mover
{
    // The basic properties of a mover class
    private Vector2 location, velocity;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    // Gives the class a GameObject to draw on the screen
    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public Mover()
    {
        findWindowLimits();
        location = new Vector2(Random.Range(minimumPos.x, maximumPos.x), Random.Range(minimumPos.y, maximumPos.y));
        velocity = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
    }

    public void Update()
    {
        // Moves the mover
        location += velocity * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

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




