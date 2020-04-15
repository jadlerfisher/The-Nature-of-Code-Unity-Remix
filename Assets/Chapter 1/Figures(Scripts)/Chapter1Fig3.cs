using UnityEngine;

// In this example, we will be subtracting Vector2's from one another
// The following example demonstrates vector subtraction by taking the difference
// between two points: the mouse location and the center sphere

public class Chapter1Fig3 : MonoBehaviour
{
    // These objects are brought in from the unity scene
    public Camera camera;
    public GameObject cursorSphere;
    public GameObject centerSphere;

    // A LineRenderer component will draw a line along our vector
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate a cursor GameObject to track the location of the mouse
        // This is a clone of the 'mover' prefab
        cursorSphere = Instantiate(cursorSphere, new Vector3(0, 0, 0), Quaternion.identity);

        // Add the Unity Component "LineRenderer" to the GameObject this script is attached to
        lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Track the Vector2 of the mouse's position and the center sphere's position
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 centerSpherePos = centerSphere.transform.position;

        // Subtract the two vectors 
        Vector2 difference = subtractVectors(mousePos, centerSpherePos);

        // Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        // Make sure the end of the line (1) appears at the new Vector3 we are creating via the method "subtractVector" 
        lineRenderer.SetPosition(0, centerSpherePos);
        lineRenderer.SetPosition(1, difference);

        //Move the cursor to that same Vector3 we are creating via the "void subtractVector" 
        cursorSphere.transform.position = mousePos;
    }
    
    // This method calculates A - B component wise
    // subtractVectors(vecA, vecB) will yield the same output as Unity's built in operator: vecA - vecB
    Vector2 subtractVectors(Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x - vectorB.x;
        float newY = vectorA.y - vectorB.y;
        return new Vector2(newX, newY);
    }
}