using UnityEngine;

// In this example, we will be subtracting Vector2's from one another
// The following example demonstrates vector subtraction by taking the difference
// between two points: the mouse location and the center sphere

public class Chapter1Fig3 : MonoBehaviour
{
    // These objects are brought in from the unity scene
    public Camera camera;
    public GameObject centerSphere;
    public GameObject followerSphere;

    // A LineRenderer component will draw a line along our vector
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
        // Track the Vector2 of the mouse's position and the center sphere's position
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        // Define a set distance that the follower will be offset by
        //Vector2 toSubtract = new Vector2(2, 3);

        // Subtract the two vectors to get the follower's position
        Vector2 followerPosition = subtractVectors(mousePos, centerSphere.transform.position);

        // Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        // Make sure the end of the line (1) appears at the new Vector3 we are creating via the method "subtractVector" 
        lineRender.SetPosition(0, centerSphere.transform.position);
        lineRender.SetPosition(1, followerPosition);

        // Update the positions of the spheres in the scene to our vectors
        //centerSphere.transform.position = Vector2.zero;
        followerSphere.transform.position = followerPosition;
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