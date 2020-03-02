using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig5 : MonoBehaviour
{
    //Create variables to mark the origin of our line
    public GameObject centerSphere;
    private Vector3 centerSpherePosition;

    //Create variables to mark the origin of our line
    public GameObject magLineOrigin;
    private Vector3   magLineOriginPosition;

    //Create variables to track the mouse gameObject and position
    private Vector3 mousePos;
    public GameObject mouseCursor;

    //Create variables for rendering the line between two vectors
    private GameObject lineDrawing;
    private LineRenderer lineRender;

    //Create variables for rendering the magnitude line between two vectors
    private GameObject magLineDrawing;
    private LineRenderer magLineRender;



    //Float coordinates for our new vector3 we create via  "void subtractVector"
    private float x, y, z;

    //Float coordinates for our new vector3 we create via  "void subtractVector"
    private Vector3 subtractedVector;

    //Float to record the Magntiude of the line
    private float vectorMagnitude;

    // Start is called before the first frame update
    void Start()
    {

        // Get the Vector3 (x,y,z) float coordinates of the center transform
        centerSpherePosition = centerSphere.transform.position;
        magLineOriginPosition = magLineOrigin.transform.position;

        //Instantiate a cursor GameObject to track the location of the mouse
        mouseCursor = Instantiate(mouseCursor, new Vector3(0, 0, 0), Quaternion.identity);

        // Create a GameObject that will be the line
        lineDrawing = new GameObject();
        magLineDrawing = new GameObject();

        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing. We will see a bright pink line.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
        magLineRender = magLineDrawing.AddComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //Track the Vector3 of the mouse's position
        mousePos = Input.mousePosition;

        //Get the center of the transform
        centerSpherePosition = centerSphere.transform.position;

        //Subtract the vector of the center from that of the mice position via "void subtractVector"
        subtractVector(mousePos, centerSpherePosition);

        //Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        //Make sure the end of the line (1) appears at the new Vector3 we are creating via the "void subtractVector" 
        lineRender.SetPosition(0, centerSpherePosition);
        lineRender.SetPosition(1, subtractedVector);

        //Move the cursor to that same Vector3 we are creating via the "void subtractVector" 
        mouseCursor.transform.position = new Vector3(x, y, z);

        //Let's create another line to parallel the magnitude of our original line
        //Begin rendering the line between the two objects. Set the first point (0) at magLineOriginPosition
        //Make sure the end of the line (1) appears at the new Vector3 we are creating via the "void subtractVector" 
        magLineRender.SetPosition(0, magLineOriginPosition);
        magLineRender.SetPosition(1, new Vector3 (vectorMagnitude, vectorMagnitude, vectorMagnitude));

    }

    void subtractVector(Vector3 originalV3, Vector3 v3)
    {

        // Dividing the subtraction by 100 to keep the cursor on the screen in this example
        x = (originalV3.x - v3.x) / 100;
        y = (originalV3.y - v3.y) / 100;
        z = (originalV3.z - v3.z) / 100;

        subtractedVector = new Vector3(x, y, z);

        //Returns the length of this vector (Read Only).
        //The length of the vector is square root of(x * x + y * y + z * z).       
        vectorMagnitude = Vector3.Magnitude(subtractedVector);
    }
}
