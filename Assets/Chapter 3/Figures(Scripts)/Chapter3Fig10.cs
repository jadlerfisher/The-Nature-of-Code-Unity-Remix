using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig10 : MonoBehaviour
{
    // Get the gravity strength and the pendulum from the scene
    public float gravity;
    public float damping;
    public float radius;
    public float startingAngleDegrees;

    Pendulum pendulum;

    void Start()
    {
        // Create a new instance of the Pendulum behavior
        pendulum = gameObject.AddComponent<Pendulum>();
        // Position the pivot for the pendulum at the top center of the camera
        pendulum.pivot = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height));
        // Pass the values into the new pendulum
        pendulum.gravity = gravity;
        pendulum.damping = damping;
        pendulum.radius = radius;
        // Adjust the angle since 0 degrees should point down, not up
        pendulum.angle = (180 - startingAngleDegrees) * Mathf.Deg2Rad;
    }
}
