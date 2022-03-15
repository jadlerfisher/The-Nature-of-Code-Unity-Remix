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
        lineRenderer.material = new Material(Shader.Find("Diffuse"));
        lineRenderer.startWidth = .5f;
        lineRenderer.endWidth = .5f;

        // Create a sphere for the bob of the pendulum
        GameObject pendulumBob = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pendulumBob.GetComponent<SphereCollider>().enabled = false;
        Destroy(pendulumBob.GetComponent<SphereCollider>());

        // Support for WebGL
        Renderer r = pendulumBob.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        bobTransform = pendulumBob.transform;
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
        bobTransform.position = pivot + new Vector2(radius * Mathf.Sin(angle), radius * Mathf.Cos(angle));
        lineRenderer.SetPosition(0, pivot);
        lineRenderer.SetPosition(1, bobTransform.position);
    }
}
