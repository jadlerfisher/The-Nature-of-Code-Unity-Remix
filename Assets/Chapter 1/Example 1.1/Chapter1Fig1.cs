﻿using System.Collections;
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
    private Vector2 maximumPos;
    private float xMin, yMin, xMax, yMax;

    // A Variable to represent our mover in the scene
    private GameObject mover;

    // Start is called before the first frame update
    void Start()
    {
        // Call FindWindowLimits() on start
        FindWindowLimits();

        // We can now properly assign the Min and Max for the scene
        xMin = -maximumPos.x;
        xMax = maximumPos.x;
        yMin = -maximumPos.y;
        yMax = maximumPos.y;

        // We now can set the mover as a primitive sphere in unity
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    // Update is called once per frame
    void Update()
    {
        // Each frame, we will check to see if the mover has touched a border
        // We check if the X/Y position is greater than the max position OR if it's less than the minimum position
        bool xHitBorder = x > xMax || x < xMin;
        bool yHitBorder = y > yMax || y < yMin;

        // If the mover has hit at all, we will mirror it's speed with the corrisponding boarder
        if (xHitBorder) 
        {
            xSpeed = -xSpeed;
        }
        if (yHitBorder) 
        {
            ySpeed = -ySpeed;
        }

        // Lets now update the location of the mover
        x += xSpeed;
        y += ySpeed;

        // Now we apply the positions to the mover to put it in it's place
        mover.transform.position = new Vector2(x, y);
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

}