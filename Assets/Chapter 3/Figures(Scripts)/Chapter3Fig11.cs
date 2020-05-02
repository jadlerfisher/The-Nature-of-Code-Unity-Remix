using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig11 : MonoBehaviour
{
    // Get spring values from the inspector
    public float springConstantK = 3.5f;
    public float restLength = 3f;
    public Transform anchorTransform;
    public Rigidbody bobBody;

    // Start is called before the first frame update
    void Start()
    {
        // Add a new spring at the start of runtime
        Spring3_11 spring = gameObject.AddComponent<Spring3_11>();
        spring.anchor = anchorTransform;
        spring.connectedBody = bobBody;
        spring.restLength = restLength;
        spring.springConstantK = springConstantK;

        // Add the click-drag behavior
        ClickDragBody3_11 mouseDrag = bobBody.gameObject.AddComponent<ClickDragBody3_11>();
        mouseDrag.body = bobBody;
        mouseDrag.radius = 1;
    }
}

public class Spring3_11 : MonoBehaviour
{
    // Properties that need to be assigned by the inspector or other scripts
    public Transform anchor;
    public Rigidbody connectedBody;
    public float restLength = 1;
    public float springConstantK = 0.1f;

    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Diffuse"));
        lineRenderer.widthMultiplier = 0.5f;
    }

    void FixedUpdate()
    {
        // Get the difference Vector3 between the anchor and the body
        Vector3 force = connectedBody.position - anchor.position;

        float currentLength = force.magnitude;
        float stretchLength = currentLength - restLength;

        // Reverse the direction of the force arrow and set its length
        // based on the spring constant and stretch
        force = -force.normalized * springConstantK * stretchLength;

        // Apply the force to the connected body relative to time
        connectedBody.AddForce(force * Time.fixedDeltaTime, ForceMode.Impulse);
        // Draw the line along the spring
        lineRenderer.SetPosition(0, anchor.position);
        lineRenderer.SetPosition(1, connectedBody.position);
    }
}

public class ClickDragBody3_11 : MonoBehaviour
{
    public Rigidbody body;
    public float radius;

    bool isDragging = false;
    Material defaultMaterial;
    Material mouseOverMaterial;
    Renderer bodyRenderer;

    void Start()
    {
        bodyRenderer = body.gameObject.GetComponent<Renderer>();
        defaultMaterial = bodyRenderer.material;
        // WebGL Support
        mouseOverMaterial = new Material(Shader.Find("Diffuse"));
        mouseOverMaterial.color = Color.green;
    }

    void Update()
    {
        Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(isDragging)
        {
            body.position = mouseLocation;
            if(Input.GetMouseButtonUp(0))
            {
                body.position = mouseLocation;
                body.velocity = Vector3.zero;
                isDragging = false;
            }
        }
        else
        {
            if(Vector2.Distance(mouseLocation, body.position) < radius)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    isDragging = true;
                }
                bodyRenderer.material = mouseOverMaterial;
            }
            else
            {
                bodyRenderer.material = defaultMaterial;
            }
        }
    }
}