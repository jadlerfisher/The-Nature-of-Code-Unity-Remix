using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig7 : MonoBehaviour
{
    [SerializeField] float maxSpeed = 2, maxForce = 2;

    private List<Vehicle> vehicles; // Declare a List of Vehicle objects.
    private Vector2 maximumPos;

    void Start()
    {
        FindWindowLimits();
        vehicles = new List<Vehicle>(); // Initilize and fill the List with a bunch of Vehicles
        for (int i = 0; i < 100; i++) {
            float ranX = Random.Range(-maximumPos.x, maximumPos.x);
            float ranY = Random.Range(-maximumPos.y, maximumPos.y);
            vehicles.Add(new Vehicle(new Vector2(ranX, ranY), -maximumPos, maximumPos, maxSpeed, maxForce));
        }
    }

    void Update()
    {
        foreach (Vehicle v in vehicles) 
        {
            v.Separate(vehicles);
            v.CheckEdges();
        }

        if (Input.GetMouseButton(0)) 
        {
            Vector2 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            vehicles.Add(new Vehicle(mousePos, -maximumPos, maximumPos, maxSpeed, maxForce));
        }
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -5);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}

class Vehicle 
{
    // To make it easier on ourselves, we use Get and Set as quick ways to get the location of the vehicle
    public Vector2 location
    { 
        get { return myVehicle.transform.position; }
        set { myVehicle.transform.position = value; }        
    }
    public Vector2 velocity
    {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }

    private float maxSpeed, maxForce;
    private Vector2 minPos, maxPos;
    private GameObject myVehicle;
    private Rigidbody rb;

    public Vehicle(Vector2 initPos, Vector2 _minPos, Vector2 _maxPos, float _maxSpeed, float _maxForce)
    {
        minPos = _minPos - Vector2.one;
        maxPos = _maxPos + Vector2.one;
        maxSpeed = _maxSpeed;
        maxForce = _maxForce;

        myVehicle = GameObject.CreatePrimitive(PrimitiveType.Sphere); // Give our vehicle some visuals in our world.
        Renderer renderer = myVehicle.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.red;

        myVehicle.transform.position = new Vector2(initPos.x, initPos.y); // Set the vehicle's initial position.
        myVehicle.AddComponent<Rigidbody>(); // Add rigidbody so we can add forces to the vehicle.

        Object.Destroy(myVehicle.GetComponent<SphereCollider>());

        rb = myVehicle.GetComponent<Rigidbody>();
        rb.useGravity = false; // Remember to ignore gravity!
    }

    public void CheckEdges() 
    {
        if (location.x > maxPos.x)
        {
            location = new Vector2(minPos.x, location.y);
        }
        else if (location.x < minPos.x) 
        {
            location = new Vector2(maxPos.x, location.y);
        }

        if (location.y > maxPos.y)
        {
            location = new Vector2(location.x, minPos.y);
        }
        else if (location.y < minPos.y)
        {
            location = new Vector2(location.x, maxPos.y);
        }
    }

    public void Separate(List<Vehicle> vehicles) 
    {
        Vector2 sum = Vector2.zero; // Start with a blank Vector2.
        int count = 0; // We have to keep track of how many Vehicles are too close.

        // Note how the desired separation is based on the Vehicle's size.
        float desiredSeparation = myVehicle.transform.localScale.x * 2; 

        foreach (Vehicle other in vehicles) 
        {
            // What is the distance between this vehicle and another Vehicle?
            float d = Vector2.Distance(other.location, location);

            if ((d > 0) && (d < desiredSeparation)) 
            {
                // Any code here will be executed if the Vehicle is within desired seperation.
                Vector2 diff = location - other.location; // A Vector2 pointing away from the other’s location.
                diff.Normalize();

                /* What is the magnitude of the PVector pointing away
                 * from the other vehicle? The closer it is, the more we 
                 * should flee. The farther, the less. So we divide by the
                 * distance to weight it appropriately. */
                diff /= d;

                sum += diff; // Add all the vectors together and increment the count
                count++;
            }
        }

        /* We have to make sure we found at least one close vehicle.
         * We don't want to bother doing anything if nothing is
         * too close (not to mention we can't divide by zero!) */
        if (count > 0) 
        {
            sum /= count;

            sum *= maxSpeed; // Scale average to maxSpeed (this becomes desired).

            Vector2 steer = sum - velocity; // Reynold's steering formula
            steer = Vector2.ClampMagnitude(steer, maxForce); // Clamp to the maximum force.

            rb.AddForce(steer); // Apply the force to the Vehicle's acceleration.
        }
    }
}