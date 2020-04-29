using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    // Public variables can be modified by other scripts
    public Vector2 pivot = Vector2.zero;
    public float radius = 1;
    public float angle = 0;
    public float damping = 1;
    public float gravity = 0;
    // Other scripts cannot see private variables
    LineRenderer lineRenderer;
    Transform bobTransform;
    float aVelocity;
    float aAcceleration;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the pendulum in a resting state
        aVelocity = 0;
        aAcceleration = 0;

        // Add the Unity Component "LineRenderer" to the GameObject lineDrawing. We will see a bright pink line.
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        // Create a sphere for the bob of the pendulum
        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newObject.GetComponent<SphereCollider>().enabled = false;
        Destroy(newObject.GetComponent<SphereCollider>());
        bobTransform = newObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the force being applied on this pendulum
        aAcceleration = -gravity * Mathf.Sin(angle);
        aVelocity += aAcceleration * Time.deltaTime;
        angle += aVelocity;
        // Apply damping to slow down the velocity over time
        aVelocity *= damping;

        // Reposition the bob by converting from polar coordinates
        bobTransform.position = pivot + new Vector2(
            radius * Mathf.Sin(angle),
            radius * Mathf.Cos(angle)
        );
        lineRenderer.SetPosition(0, pivot);
        lineRenderer.SetPosition(1, bobTransform.position);
    }
}
