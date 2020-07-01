using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example 6.3 Stay Within Walls, Steering Behavior
/// </summary>

public class Chapter6Fig3 : MonoBehaviour
{
    // Our cone asset found in Extended Primitives folder
    [SerializeField] private GameObject vehiclePrefab;

    [Header("Sets vehicle's acceleration on Start")]
    [Tooltip("This number is multiplied by fixedDeltaTime, a very small number, so this number needs to be large ie 300x300")]
    [SerializeField] private Vector2 startingAcceleration;

    [Header("How far from screen bounds before steering away")]
    [Tooltip("In meters/Unity Units")]
    [SerializeField][Range(3,10)] private float distance;

    [Header("Vehicle cannot go above this speed")]
    [Tooltip("In meters/Unity Units")]
    [SerializeField] private float maxSpeed;

    [Header("How much force can we add to steering, or how sharply we can turn")]
    [SerializeField] private float maxForce;

    // Our vehicle, a mover with velocity, acceleration and steering
    private Ch6Fig3Vehicle vehicle;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate vehicle at origin. Use exposed variable for distance, or how far from the wall before we steer.
        // Instantiate make a copy of our cone prefab
        GameObject vehicleRepresentation = Instantiate(vehiclePrefab, Vector2.zero, Quaternion.identity);        
        vehicle = new Ch6Fig3Vehicle(vehicleRepresentation, startingAcceleration, distance, maxSpeed, maxForce);
    }

    // FixedUpdate is called 50 times per second per project default
    void FixedUpdate()
    {
        // Update vehicle behavior
        vehicle.Update();

        // Stays within screen bounds
        vehicle.StayWithinWalls(); 
    }
}

public class Ch6Fig3Vehicle
{
    // Visual representation of our vehicle
    private GameObject vehicleObject; 
    private Vector2 location;
    public Vector2 velocity { get; private set; } 
    private Vector2 acceleration;

    // How fast we can go, velocity cannot go higher
    private float maxSpeed;

    // How much to steer in the opposite direction when reaching screen bounds. Higher numbers mean sharper turns.
    private float maxForce;

    // Top right of the screen
    private Vector2 maximumPos;

    // Bottom left of the screen
    private Vector2 minimumPos;

    // How far away from each wall before we start steering away, in meters
    private float distance; 

    public Ch6Fig3Vehicle(GameObject _vehicle, Vector2 _acceleration, float _distance, float _maxSpeed, float _maxForce)
    {
        distance = _distance;

        // Our vehicle will be a cone
        vehicleObject = _vehicle;

        // Gives us min and max Pos, or the edges of the screen
        findWindowLimits();

        // Let's assign our initial position to origin
        location = Vector2.zero;

        // The sphere will match our position
        vehicleObject.transform.position = location;

        // Velocity is initialized to zero, this is changed in first frame update due to setting our acceleration
        velocity = Vector2.zero;

        // Setting our initial acceleration to constructor parameters. Acceleration gets reset at first Update
        acceleration = _acceleration;

        // Neither Velocity.x or Velocity.y can exceed maxSpeed
        maxSpeed = _maxSpeed;

        // Steering force. Higher numbers means sharper turn
        maxForce = _maxForce;
    }

    public void Update() // Gets called every FixedUpdate
    {
        // We multiplay these values with Time.fixedDelaTime to ensure frame rate independence.
        // Although FixedUpdate is fixed, environments that can't keep up with 50 Frames Per Second 
        // will experience different behavior unless we do this.

        velocity += acceleration * Time.fixedDeltaTime;

        // We clamp velocity's magnitude so its values don't exceed maxSpeed
        Vector2.ClampMagnitude(velocity, maxSpeed);

        // Update location according to velocity, update vehicle representation
        location += velocity * Time.fixedDeltaTime;
        vehicleObject.transform.position = location;

        // Use transform.LookAt toward where we're going
        vehicleObject.transform.LookAt(vehicleObject.transform.position + (Vector3)velocity);

        // Adjust the x rotation of the object by 90 degrees
        Vector3 vehicleEularAngles = vehicleObject.transform.rotation.eulerAngles;
        vehicleObject.transform.rotation = Quaternion.Euler(vehicleEularAngles.x + 90, vehicleEularAngles.y, vehicleEularAngles.z); 
        acceleration *= 0;        
    }

    public void StayWithinWalls() 
    {
        // If the vehicle comes within distance of a wall, it desires to move at 
        // Maximum speed in the opposite direction of the wall.

        // Left side of the screen
        if (location.x < minimumPos.x + distance)
        {
            // Make a desired vector that wants to go in the opposite direction of the wall
            // as fast as possible (maxSpeed);
            Vector2 desired = new Vector2(maxSpeed, velocity.y);
            Vector2 steer = desired - velocity;
            // But can only steer away from the wall as much as maxForce will let us
            Vector2.ClampMagnitude(steer, maxForce);
            applyForce(steer);
        }
        // Right side of the screen
        else if (location.x > maximumPos.x - distance)
        {
            Vector2 desired = new Vector2(-maxSpeed, velocity.y);
            Vector2 steer = desired - velocity;
            Vector2.ClampMagnitude(steer, maxForce);
            applyForce(steer);
        }

        // Bottom of the screen
        if (location.y < minimumPos.y + distance)
        {
            Vector2 desired = new Vector2(velocity.x, maxSpeed);
            Vector2 steer = desired - velocity;
            Vector2.ClampMagnitude(steer, maxForce);
            applyForce(steer);
        }
        // Top of the screen
        else if (location.y > maximumPos.y - distance)
        {
            Vector2 desired = new Vector2(velocity.x, -maxSpeed);
            Vector2 steer = desired - velocity;
            Vector2.ClampMagnitude(steer, maxForce);
            applyForce(steer);
        }
    }
    
    private void applyForce(Vector2 force)
    {
        // Newton's second law with force accumulation.
        acceleration += force;
    }

    private void findWindowLimits()
    {
        // Set camera settings
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;

        // Finds the space of the screen (in pixels), translates them into space of the screen in meters or Unity Units
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        minimumPos = -maximumPos;        
    }
}
