using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example 6.4: Flow Field Following
/// </summary>

public class Ch6Fig4 : MonoBehaviour
{
    [SerializeField] private GameObject vehicleRepresentation;

    private Ch6Fig4Vehicle vehicle;
    private Ch6Fig4FlowField field;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate our vehicle, location doesn't matter as it will get overwritten
        GameObject vehicleObject = Instantiate(vehicleRepresentation, Vector3.zero, Quaternion.identity);
        vehicle = new Ch6Fig4Vehicle(vehicleObject);

        // Makes a grid of random vectors that moves our vehicle around
        field = new Ch6Fig4FlowField();
    }

    private void FixedUpdate()
    {        
        vehicle.Follow(field);
        vehicle.Update();
    }
}

public class Ch6Fig4Vehicle
{
    // This Sphere will represent our vehicle
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
        Vector2 steerVelocity = desiredVelocity - velocity;
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
        // FixedUpdate is called 50 times per second per project default
        velocity += acceleration * Time.fixedDeltaTime;
        Vector2.ClampMagnitude(velocity, maxSpeed);
        location += velocity * Time.fixedDeltaTime;

        // Use transform.LookAt toward where we're going
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
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        minimumPos = -maximumPos;
    }
}

public class Ch6Fig4FlowField
{
    // A flow field is a two-dimensional array of Vector2s
    private Vector2[,] field;

    // Columns and rows to line the grid
    private int columns, rows;

    // Resolution of grid relative to screen width and height
    private int resolution;

    public Ch6Fig4FlowField()
    {
        resolution = 10;
        columns = Screen.width / resolution;        
        rows = Screen.height / resolution;
        field = new Vector2[columns, rows];
        initializeFlowField();        
    }

    private void initializeFlowField()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++) 
            {
                // Our grid consists of random Vector2's pushing our vehicle around
                Vector2 v = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f,1f)); 

                // Not necessary to Normalize in this situation
                v.Normalize();                
                field[i,j] = v;
            }
        }
    }

    public Vector2 Lookup(Vector2 _lookUp)
    {
        // Repeatedly checks to see what Vector2 we're sitting on
        int column = (int)Mathf.Clamp(_lookUp.x, 0, columns - 1);
        int row = (int)Mathf.Clamp(_lookUp.y, 0, rows - 1);
        return field[column, row];
    }
}
