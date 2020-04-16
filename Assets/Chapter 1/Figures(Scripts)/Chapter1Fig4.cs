using UnityEngine;

// In this example, we will be scaling vector2's via multiplication.
// The following example draws a vector from a central location to the mouse,
// and either overshoots or undershoots the mouse based on the scale factor.

public class Chapter1Fig4 : MonoBehaviour
{
    // These objects are brought in from the unity scene
    public Camera camera;
    public GameObject centerSphere;
    public GameObject cursorSphere;

    //Create variables for rendering the line between two vectors
    private LineRenderer lineRender;

    // Start is called before the first frame update
    void Start()
    {
        // Add the Unity Component "LineRenderer" to the GameObject this script is attached to
        lineRender = gameObject.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Track the Vector2 of the mouse's position and the center sphere's position
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 centerPos = centerSphere.transform.position;
        // Define a scaling coefficient to multiply x and y by
        float scaleFactor = 0.5f;

        // Get a vector for the distance between the two spheres
        Vector2 differenceVector = subtractVectors(mousePos, centerPos);
        // Scale the vector
        Vector2 scaledVector = multiplyVector(differenceVector, scaleFactor);
        // Add the scaled vector back to the center position
        Vector2 scaledMousePos = addVectors(centerPos, scaledVector);
        
        // Begin rendering the line between the two objects. Set the first point (0) at the centerSphere position
        // Make sure the end of the line (1) appears at the new Vector3 we are creating
        lineRender.SetPosition(0, centerSphere.transform.position);
        lineRender.SetPosition(1, scaledMousePos);

        // Move the cursor to that same Vector3 we created
        cursorSphere.transform.position = scaledMousePos;
    }

    // This method calculates A + B component wise
    // addVectors(vecA, vecB) will yield the same output as Unity's built in operator: vecA + vecB
    Vector2 addVectors(Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x + vectorB.x;
        float newY = vectorA.y + vectorB.y;
        return new Vector2(newX, newY);
    }

    // This method calculates A - B component wise
    // subtractVectors(vecA, vecB) will yield the same output as Unity's built in operator: vecA - vecB
    Vector2 subtractVectors(Vector2 vectorA, Vector2 vectorB)
    {
        float newX = vectorA.x - vectorB.x;
        float newY = vectorA.y - vectorB.y;
        return new Vector2(newX, newY);
    }

    // This method calculates A * b component wise
    // multiplyVector(vector, factor) will yield the same output as Unity's built in operator: vector * factor
    Vector2 multiplyVector(Vector2 toMultiply, float scaleFactor)
    {
        float x = toMultiply.x * scaleFactor;
        float y = toMultiply.y * scaleFactor;
        return new Vector2(x, y);
    }
}
