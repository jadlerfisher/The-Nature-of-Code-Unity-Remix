using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example 6.4: Flow Field Following
/// </summary>

public class Ch6Fig4 : MonoBehaviour
{
    // Our cone object will be our vehicle, found in our extended primitives folder
    [SerializeField] private GameObject vehicleRepresentation;

    private Ch6Fig4Vehicle vehicle;
    private Ch6Fig4FlowField field;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate our vehicle, location doesn't matter as it will get overwritten
        // Instantiate makes a copy of our cone
        GameObject vehicleObject = Instantiate(vehicleRepresentation, Vector3.zero, Quaternion.identity);
        vehicle = new Ch6Fig4Vehicle(vehicleObject);

        // Makes a grid of random vectors that moves our vehicle around
        field = new Ch6Fig4FlowField();
    }

    // FixedUpdate runs 50 times a second per project default
    private void FixedUpdate()
    {        
        vehicle.Follow(field);
        vehicle.Update();
    }
}

public class Ch6Fig4Vehicle
{
    private GameObject vehicleRepresentation;
    private Vector2 location;
    private Vector2 velocity;
    private Vector2 acceleration;

    // How fast we can move
    private float maxSpeed;

    // How much force to put into steering
    private float maxForce;    
    private Vector2 minimumPos;
    private Vector2 maximumPos;

    public Ch6Fig4Vehicle(GameObject _vehicleObject)
    {
        vehicleRepresentation = _vehicleObject;        
        location = Vector2.zero;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        maxSpeed = 4f;
        maxForce = 0.1f;
        findWindowBounds();
    }

    public void Follow(Ch6Fig4FlowField flow)
    {        
        // What is the vector at the place we're standing in?
        Vector2 desiredVelocity = flow.Lookup(location);
        desiredVelocity *= maxSpeed;        
        Vector2 steerVelocity = desiredVelocity - velocity; // Steering is desired minus velocity
        Vector2.ClampMagnitude(steerVelocity, maxForce);
        applyForce(steerVelocity);
    }

    private void applyForce(Vector2 forceToAdd)
    {
        // Newton's second law, force gets accumulated
        acceleration += forceToAdd;
    }

    public void Update()
    {
        velocity += acceleration * Time.fixedDeltaTime;
        Vector2.ClampMagnitude(velocity, maxSpeed);
        location += velocity * Time.fixedDeltaTime;

        // Use transform.LookAt to rotate our vehicle to look toward where we're going
        vehicleRepresentation.transform.LookAt(vehicleRepresentation.transform.position + (Vector3)velocity);

        // Adjust the x rotation of the object by 90 degrees
        Vector3 vehicleEularAngles = vehicleRepresentation.transform.rotation.eulerAngles;
        vehicleRepresentation.transform.rotation = Quaternion.Euler(vehicleEularAngles.x + 90, vehicleEularAngles.y, vehicleEularAngles.z);

        // Acceleration gets reset every update
        acceleration *= 0;

        // Updates sphere representation
        vehicleRepresentation.transform.position = location;

        // If the representation goes off screen, representation and location gets reset
        stayWithinScreen();
    }

    private void stayWithinScreen()
    {
        if (location.x > maximumPos.x || location.x < minimumPos.x
         || location.y > maximumPos.y || location.y < minimumPos.y)
        {            
            location = vehicleRepresentation.transform.position = Vector2.zero;            
        }
    }

    private void findWindowBounds()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;

        // Translates screen bounds (in pixels) into meters or Unity Units
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        minimumPos = -maximumPos;
    }
}

public class Ch6Fig4FlowField
{
    // Declaring a 2D array of Vector2's
    private Vector2[,] field;

    // How many columns and how many rows in the grid?
    private int columns, rows;

    // Resolution of grid relative to window width and height in pixels
    private int resolution;

    public Ch6Fig4FlowField()
    {
        resolution = 10;
        columns = Screen.width / resolution; // Total columns equals width divided by resolution
        rows = Screen.height / resolution; // Total rows equals height divided by resolution
        field = new Vector2[columns, rows];
        initializeFlowField();        
    }

    private void initializeFlowField()
    {
        float xOff = 0;
        for (int i = 0; i < columns; i++) // Using a nested loop to hit every column and every row of the flow field
        {
            float yOff = 0;
            for (int j = 0; j < rows; j++) 
            {
                // In this example, we use Perlin noise to seed the vectors.
                float noiseValue = Mathf.PerlinNoise(xOff, yOff);

                // A C# recreation of Processing's Map function, which re-maps
                // A number from one range to another. 
                // https://processing.org/reference/map_.html
                float theta = 0 + ((Mathf.PI * 2) - 0) * ((noiseValue - 0) / (1 - 0));                                
                Vector2 v = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
                                              
                field[i,j] = v;
                yOff += 0.1f;
            }
            xOff += 0.1f;
        }
    }

    public Vector2 Lookup(Vector2 _lookUp)
    {
        // A method to return a Vector2 based on a location
        int column = (int)Mathf.Clamp(_lookUp.x, 0, columns - 1);
        int row = (int)Mathf.Clamp(_lookUp.y, 0, rows - 1);
        return field[column, row];
    }
}
