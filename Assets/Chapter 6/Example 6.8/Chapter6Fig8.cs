using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig8 : MonoBehaviour
{
    [SerializeField] float maxSpeed = 2, maxForce = 2;
    [SerializeField] float separationScale;
    [SerializeField] float seekScale;

    private List<Vehicle6_8> vehicles; // Declare a List of Vehicle objects.
    private Vector2 maximumPos;

    // Start is called before the first frame update
    void Start()
    {
        FindWindowLimits();
        vehicles = new List<Vehicle6_8>(); // Initilize and fill the List with a bunch of Vehicles
        for (int i = 0; i < 100; i++)
        {
            float ranX = Random.Range(-maximumPos.x, maximumPos.x);
            float ranY = Random.Range(-maximumPos.y, maximumPos.y);
            vehicles.Add(new Vehicle6_8(new Vector2(ranX, ranY), -maximumPos, maximumPos, maxSpeed, maxForce));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        foreach (Vehicle6_8 v in vehicles)
        {
            Vector2 seperate = v.Separate(vehicles);
            Vector2 seek = v.Seek(mousePos);

            /* These values can be whatever you want. Modify in the inspector*/
            seperate *= separationScale;
            seek *= seekScale;

            v.ApplyForce(seperate);
            v.ApplyForce(seek);
        }

        if (Input.GetMouseButton(0))
        {
            vehicles.Add(new Vehicle6_8(mousePos, -maximumPos, maximumPos, maxSpeed, maxForce));
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

class Vehicle6_8
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

    public Vehicle6_8(Vector2 initPos, Vector2 _minPos, Vector2 _maxPos, float _maxSpeed, float _maxForce)
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

        GameObject.Destroy(myVehicle.GetComponent<SphereCollider>());

        rb = myVehicle.GetComponent<Rigidbody>();
        rb.useGravity = false; // Remember to ignore gravity!
    }

    public Vector2 Seek(Vector2 target)
    {
        Vector2 desired = target - location;
        desired.Normalize();
        desired *= maxSpeed;
        Vector2 steer = desired - velocity;
        steer = Vector2.ClampMagnitude(steer, maxForce);

        return steer;
    }

    public Vector2 Separate(List<Vehicle6_8> vehicles)
    {
        Vector2 sum = Vector2.zero;
        int count = 0;

        float desiredSeperation = myVehicle.transform.localScale.x * 2;

        foreach (Vehicle6_8 other in vehicles)
        {
            float d = Vector2.Distance(other.location, location);

            if ((d > 0) && (d < desiredSeperation))
            {
                Vector2 diff = location - other.location;
                diff.Normalize();

                diff /= d;

                sum += diff;
                count++;
            }
        }

        if (count > 0)
        {
            sum /= count;

            sum *= maxSpeed;

            Vector2 steer = sum - velocity;
            steer = Vector2.ClampMagnitude(steer, maxForce);


            return steer;
        }
        return Vector2.zero;
    }

    public void ApplyForce(Vector2 force) 
    {
        rb.AddForce(force);
    }

}