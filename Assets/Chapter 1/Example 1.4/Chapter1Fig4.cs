using UnityEngine;

// In this example, we will be scaling vector2's via multiplication.
// The following example draws a vector from a central location to the mouse,
// and either overshoots or undershoots the mouse based on the scale factor.

public class Chapter1Fig4 : MonoBehaviour
{
    // These objects are brought in from the Unity scene and must be assigned in the inspector
    // [SerializeField] is used to make private variables visible in the Unity inspector
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject centerSphere;
    [SerializeField] private GameObject cursorSphere;

    //Create variables for rendering the line between two vectors
    private LineRenderer lineRender;

    // Start is called before the first frame update
    void Start()
    {
        // Add the Unity Component "LineRenderer" to the GameObject this script is attached to
        lineRender = gameObject.AddComponent<LineRenderer>();
        //We need to create a new material for WebGL
        lineRender.material = new Material(Shader.Find("Diffuse"));
    }

    // Update is called once per frame
    void Update()
    {
        // Track the Vector2 of the mouse position and the center sphere position
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 centerPos = centerSphere.transform.position;
        // Define a scaling coefficient to multiply x and y by
        float scaleFactor = 0.5f;

        // Get a vector for the distance between the two spheres
        Vector2 differenceVector = SubtractVectors(mousePos, centerPos);
        // Scale the vector
        Vector2 scaledVector = ScaleVector(differenceVector, scaleFactor);
        // Add the scaled vector back to the center position
        Vector2 scaledMousePos = AddVectors(centerPos, scaledVector);
        
        // Begin rendering the line between the two objects. Set the first point (0) at the centerSphere position
        // Make sure the end of the line (1) appears at the new Vector3 we are creating
        lineRender.SetPosition(0, centerSphere.transform.position);
        lineRender.SetPosition(1, scaledMousePos);
        // Move the cursor to that same Vector3 we created
        cursorSphere.transform.position = scaledMousePos;
    }

    // This method calculates A + B component wise
    // AddVectors(vecA, vecB) will yield the same output as Unity's built in operator: vecA + vecB
    Vector2 AddVectors(Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x + vectorB.x;
        float newY = vectorA.y + vectorB.y;
        return new Vector2(newX, newY);
    }

    // This method calculates A - B component wise
    // SubtractVectors(vecA, vecB) will yield the same output as Unity's built in operator: vecA - vecB
    Vector2 SubtractVectors(Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x - vectorB.x;
        float newY = vectorA.y - vectorB.y;
        return new Vector2(newX, newY);
    }

    // This method calculates a vector scaled by a factor component wise
    // ScaleVector(vector, factor) will yield the same output as Unity's built in operator: vector * factor
    Vector2 ScaleVector(Vector2 toMultiply, float scaleFactor)
    {
        float x = toMultiply.x * scaleFactor;
        float y = toMultiply.y * scaleFactor;
        return new Vector2(x, y);
    }
}
