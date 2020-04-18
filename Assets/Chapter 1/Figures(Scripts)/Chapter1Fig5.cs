using UnityEngine;

// In this example, we will be scaling vector2's via multiplication.
// The following example draws a vector from a central location to the mouse, the length
// of the vector between the two spheres is used to draw another vector of the same length.

public class Chapter1Fig5 : MonoBehaviour
{
    // These objects are brought in from the unity scene
    public Camera camera;
    public GameObject centerSphere;
    public GameObject cursorSphere;

    // Create variables for rendering the line between two vectors
    private LineRenderer lineRenderer;
    private LineRenderer magLineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Create two new line renderers. We must create new GameObjects since one
        // object cannot accept more than one of this component
        GameObject newA = new GameObject();
        GameObject newB = new GameObject();
        lineRenderer = newA.AddComponent<LineRenderer>();
        magLineRenderer = newB.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Track the Vector2 of the mouse's position and the center sphere's position
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 centerPos = centerSphere.transform.position;
        // Move the cursor sphere directly to the mouse
        cursorSphere.transform.position = mousePos;

        // Calculate the magnitude(the distance between the two spheres)
        Vector2 differenceVector = subtractVectors(mousePos, centerPos);
        float magnitude = magnitudeOf(differenceVector);

        // Render the line between the spheres directly 
        lineRenderer.SetPosition(0, centerPos);
        lineRenderer.SetPosition(1, mousePos);

        // Render a bar in the top left of the screen with the same length as the vector
        Vector2 cameraTopLeft = camera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        magLineRenderer.SetPosition(0, cameraTopLeft);
        // Use of Unity's built in Vector2 operators:
        magLineRenderer.SetPosition(1, cameraTopLeft + magnitude * Vector2.right);
    }

    // This method finds the length of a vector using pythagoras theorem
    // magnitudeOf(vec) will yield the same output as Unity's built in property vect.magnitude
    float magnitudeOf(Vector2 vector)
    {
        return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
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
