﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chapter1Fig2 : MonoBehaviour
{
    // Variables for the location and speed of mover
    private Vector2 location = new Vector2(0f, 0f);
    private Vector2 velocity = new Vector2(0.1f, 0.1f);

    // Variables to limit the mover within the screen space
    private Vector2 maximumPos;

    // A Variable to represent our mover in the scene
    private GameObject mover;

    // Start is called before the first frame update
    void Start()
    {
        // On start we run our FindWindowLimits() method
        FindWindowLimits();

        // We now can set the mover as a primitive sphere in unity
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    // Update is called once per frame
    void Update()
    {
        // Each frame, we will check to see if the mover has touched a border
        // We check if the X/Y position is greater than the max position OR(||) if it's less than the minimum position
        bool xHitBorder = location.x > maximumPos.x || location.x < -maximumPos.x;
        bool yHitBorder = location.y > maximumPos.y || location.y < -maximumPos.y;

        // If the mover has hit a border, we will mirror its speed along the corresponding border axis
        if (xHitBorder)
        {
            velocity.x = -velocity.x;
        }
        if (yHitBorder)
        {
            velocity.y = -velocity.y;
        }

        // Lets now update the location of the mover
        location += velocity;

        // Now we apply the updated position to the mover
        mover.transform.position = new Vector2(location.x, location.y);
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