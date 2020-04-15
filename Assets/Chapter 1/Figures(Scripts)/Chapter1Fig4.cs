using UnityEngine;

// In this example, we will be subtracting Vector3's from one another
//The following example demonstrates vector subtraction by taking the difference between two points—the mouse location and the center sphere

public class Chapter1Fig4 : MonoBehaviour
{

    //Create variables to mark the origin of our line
    public Camera camera;
    public GameObject centerSphere;
    private Vector3 centerSpherePosition;

    //Create variables to track the mouse gameObject and position
    private Vector3 mousePos;
    public GameObject mouseCursor;

    //Create variables for rendering the line between two vectors
    private GameObject lineDrawing;
    private LineRenderer lineRender;


    // Start is called before the first frame update
    void Start()
    {

        // Get the Vector3 (x,y,z) float coordinates of the center transform
        centerSpherePosition = centerSphere.transform.position;

        //Instantiate a cursor GameObject to track the location of the mouse
        mouseCursor = Instantiate(mouseCursor, new Vector3(0, 0, 0), Quaternion.identity);

        // Create a GameObject that will be the line
        lineDrawing = new GameObject();

        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing. We will see a bright pink line.
        lineRender = lineDrawing.AddComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //Track the Vector3 of the mouse's position
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 centerSpherePos = centerSphere.transform.position;

        //Get the center of the transform
        centerSpherePosition = centerSphere.transform.position;

        //Subtract the vector of the center from that of the mice position via "void subtractVector"
        Vector2 sub = subtractVectors(mousePos,centerSpherePosition);
        
        //Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        //Make sure the end of the line (1) appears at the new Vector3 we are creating via the "void subtractVector" 
        lineRender.SetPosition(0, centerSpherePosition);
        lineRender.SetPosition(1, sub);

        //Move the cursor to that same Vector3 we are creating via the "void subtractVector" 
        mouseCursor.transform.position = sub;

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
