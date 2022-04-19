using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example 6.4: Flow Field Following
/// </summary>

public class Chapter6Fig4 : MonoBehaviour
{
    // Our cone object will be our vehicle, found in our extended primitives folder
    [SerializeField] private GameObject vehicleRepresentation;
    [SerializeField] private GameObject flowArrow;

    private Ch6Fig4Vehicle vehicle;
    private Ch6Fig4FlowField field;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate our vehicle, location doesn't matter as it will get overwritten
        // Instantiate makes a copy of our cone
        GameObject vehicleObject = Instantiate(vehicleRepresentation);
        vehicle = new Ch6Fig4Vehicle(vehicleObject);

        // Makes a grid of random vectors that moves our vehicle around
        field = new Ch6Fig4FlowField(flowArrow);
    }

    void Update()
    {
        field.ShowFlowField();
    }

    // FixedUpdate runs 50 times a second per project by default
    private void FixedUpdate()
    {
        vehicle.Follow(field);
        vehicle.UpdatePosition();
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
    private Vector2 maximumPos;

    public Ch6Fig4Vehicle(GameObject _vehicleObject)
    {
        FindWindowLimits();
        vehicleRepresentation = _vehicleObject;        
        location = Vector2.zero;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        maxSpeed = 4f;
        maxForce = 0.1f;
    }

    public void Follow(Ch6Fig4FlowField flow)
    {
        // What is the vector at the place we're standing in?
        //Debug.Log(location);
        Vector2 desiredVelocity = flow.Lookup(location);
        desiredVelocity *= maxSpeed;        
        Vector2 steerVelocity = desiredVelocity - velocity; // Steering is desired minus velocity
        Vector2.ClampMagnitude(steerVelocity, maxForce);
        ApplyForce(steerVelocity);
    }

    private void ApplyForce(Vector2 forceToAdd)
    {
        // Newton's second law, force gets accumulated
        acceleration += forceToAdd;
    }

    public void UpdatePosition()
    {
        velocity += acceleration * Time.fixedDeltaTime;
        Vector2.ClampMagnitude(velocity, maxSpeed);
        location += velocity * Time.fixedDeltaTime;

        // Use transform.LookAt to rotate our vehicle to look toward where we're going
        vehicleRepresentation.transform.LookAt(vehicleRepresentation.transform.position + (Vector3)velocity);

        // Adjust the x rotation of the object by 90 degrees
        Vector3 vehicleEulerAngles = vehicleRepresentation.transform.rotation.eulerAngles;
        vehicleRepresentation.transform.rotation = Quaternion.Euler(vehicleEulerAngles.x + 90, vehicleEulerAngles.y, vehicleEulerAngles.z);

        // Acceleration gets reset every update
        acceleration *= 0;

        // Updates sphere representation
        vehicleRepresentation.transform.position = location;

        // If the representation goes off screen, representation and location gets reset
        CheckEdges();
    }

    // Check if location crosses any border of the screen, relocate location to opposite border
    private void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x = -maximumPos.x;
            
        }
        else if (location.x < -maximumPos.x)
        {
            location.x = maximumPos.x;
            location.y = Random.Range(-maximumPos.y, maximumPos.y);
        }
        if (location.y > maximumPos.y)
        {
            location.y = -maximumPos.y;
        }
        else if (location.y < -maximumPos.y)
        {
            location.y = maximumPos.y;
        }
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}

public class Ch6Fig4FlowField
{
    // Declaring a 2D array of Vector2's
    private Vector2[,] field;

    public List<GameObject> flowIndicators = new List<GameObject>();

    // How many columns and how many rows in the grid?
    private int columns, rows;

    // Resolution of grid relative to window width and height in pixels
    private int resolution;

    // Maximum bounds of the screen
    private Vector3 maximumPos;

    public Ch6Fig4FlowField(GameObject flowArrow)
    {
        FindWindowLimits();
        resolution = 30;
        columns = Screen.width / resolution; // Total columns equals width divided by resolution
        rows = Screen.height / resolution; // Total rows equals height divided by resolution
        field = new Vector2[columns, rows];
        InitializeFlowField(flowArrow);        
    }

    private void InitializeFlowField(GameObject flowArrow)
    {
        // Using a nested loop to hit every column and every row of the flow field
        float xOff = 0;
        for (int i = 0; i < columns; i++)
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

                // Map values i and j to minimum and maximum bounds of viewport
                float x = -maximumPos.x + (i - 0) * ((maximumPos.x - -maximumPos.x) / ((columns - 1) - 0));
                float y = -maximumPos.y + (j - 0) * ((maximumPos.y - -maximumPos.y) / ((rows - 1) - 0));

                // Instantiate flow indicator at each point in grid
                GameObject flowIndicator = Object.Instantiate(flowArrow);
                flowIndicator.name = $"Indicator{i}_{j}_{v}";
                flowIndicator.transform.localScale = new Vector3(.3f, .3f, .3f);
                flowIndicator.transform.position = new Vector2(x, y);
                
                // Set rotation of flow indicator to match the vector at each position
                flowIndicator.transform.rotation = Quaternion.LookRotation(v);
                Vector3 indicatorEulerAngles = flowIndicator.transform.rotation.eulerAngles;
                flowIndicator.transform.rotation = Quaternion.Euler(indicatorEulerAngles.x + 90, indicatorEulerAngles.y, indicatorEulerAngles.z);

                // Add newly instantiated indicator to a list
                flowIndicators.Add(flowIndicator);

                yOff += 0.1f;
            }
            xOff += 0.1f;
        }
    }

    public Vector2 Lookup(Vector2 _lookUp)
    {
        float x = 0 + (_lookUp.x - -maximumPos.x) * ((columns - 1 - 0) / (maximumPos.x - -maximumPos.x));
        float y = 0 + (_lookUp.y - -maximumPos.y) * ((rows - 1 - 0) / (maximumPos.y - -maximumPos.y));

        // A method to return a Vector2 based on a location
        int column = (int)Mathf.Clamp(x, 0, columns - 1);
        int row = (int)Mathf.Clamp(y, 0, rows - 1);
        return field[column, row];
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    public void ShowFlowField()
    {
        // Set indicators active if holding space, set inactive if not
        if (Input.GetKey(KeyCode.Space))
        {
            // Loop through flowIndicators list and set active state for each item
            for (int i = 0; i < flowIndicators.Count; i++)
            {
                flowIndicators[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < flowIndicators.Count; i++)
            {
                flowIndicators[i].SetActive(false);
            }
        }
    }
}
