using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig10 : MonoBehaviour
{
    // Get the gravity strength and the pendulum from the scene
    [SerializeField] float gravity;
    [SerializeField] float damping;
    [SerializeField] float radius;
    [SerializeField] float startingAngleDegrees;

    Pendulum pendulum;

    Vector2 maximumPos;

    void Start()
    {
        FindWindowLimits();

        // Create a new instance of the Pendulum behavior
        pendulum = gameObject.AddComponent<Pendulum>();

        // Position the pivot for the pendulum at the top center of the camera
        pendulum.pivot = new Vector2(0, maximumPos.y);

        // Pass the values we assigned into the new pendulum
        pendulum.gravity = gravity;
        pendulum.damping = damping;
        pendulum.radius = radius;

        // Adjust the angle since 0 degrees should point down, not up
        pendulum.angle = (180 - startingAngleDegrees) * Mathf.Deg2Rad;
    }

    private void FindWindowLimits()
    {
        // Start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // For correct functionality, we set the camera x and y position to 0, 0
        Camera.main.transform.position = new Vector3(0, 0, -10);
        // Next we grab the minimum and maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
