using UnityEngine;

// In this example, we will be subtracting Vector2's from one another
// The following example demonstrates vector subtraction by taking the difference
// between two points: the mouse location and the center sphere

public class Chapter1Fig3 : MonoBehaviour
{
    // These objects are brought in from the Unity scene and must be assigned in the inspector
    // [SerializeField] is used to make private variables visible in the Unity inspector
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject centerSphere;
    [SerializeField] private GameObject cursorSphere;

    // A LineRenderer component will draw a line along our vector
    private LineRenderer lineRender;

    // Start is called before the first frame update
    void Start()
    {
        // Add the Unity Component "lineRenderer" to the GameObject this script is attached to
        lineRender = gameObject.AddComponent<LineRenderer>();

        // We need to create a new material for WebGL
        lineRender.material = new Material(Shader.Find("Diffuse"));
    }

    // Update is called once per frame
    void Update()
    {
        // Track the Vector2 of the mouse's position and the center sphere's position
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Begin rendering the line between the two objects. Set the first point (0) at the centerSphere position
        // Make sure the end of the line (1) appears at the position of the mouse 
        lineRender.SetPosition(0, centerSphere.transform.position);
        lineRender.SetPosition(1, mousePos);

        // Update the positions of the cursor sphere in the scene to our mouse position
        cursorSphere.transform.position = mousePos;
    }
}