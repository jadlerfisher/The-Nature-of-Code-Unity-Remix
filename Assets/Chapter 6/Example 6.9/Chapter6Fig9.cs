using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig9 : MonoBehaviour
{
    public float maxSpeed = 2, maxForce = 2;

    private List<Boid> boids; // Declare a List of Vehicle objects.
    private Vector2 minimumPos, maximumPos;

    // Start is called before the first frame update
    void Start()
    {
        findWindowLimits();
        boids = new List<Boid>(); // Initilize and fill the List with a bunch of Vehicles
        for (int i = 0; i < 100; i++)
        {
            float ranX = Random.Range(minimumPos.x, maximumPos.x);
            float ranY = Random.Range(minimumPos.y, maximumPos.y);
            boids.Add(new Boid(new Vector2(ranX, ranY), minimumPos, maximumPos, maxSpeed, maxForce));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        foreach (Boid v in boids)
        {
            v.Flock(boids);
        }

        if (Input.GetMouseButton(0))
        {
            boids.Add(new Boid(mousePos, minimumPos, maximumPos, maxSpeed, maxForce));
        }
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}

class Boid
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

    public Boid(Vector2 initPos, Vector2 _minPos, Vector2 _maxPos, float _maxSpeed, float _maxForce)
    {
        minPos = _minPos - Vector2.one;
        maxPos = _maxPos + Vector2.one;
        maxSpeed = _maxSpeed;
        maxForce = _maxForce;

        myVehicle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject.Destroy(myVehicle.GetComponent<BoxCollider>());

        myVehicle.transform.position = new Vector2(initPos.x, initPos.y);

        myVehicle.AddComponent<Rigidbody>();
        rb = myVehicle.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false; // Remember to ignore gravity!

        myVehicle.transform.localScale = new Vector3(1, 1, 1.5f);

        
    }

    private void checkBounds() {
        if (location.x > maxPos.x) 
        {
            location = new Vector2(minPos.x, location.y);
        } else if (location.x < minPos.x)
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
        Vector2 lookat = location + velocity;
        myVehicle.transform.LookAt(lookat);
    }

    public void Flock(List<Boid> boids) 
    {
        Vector2 sep = Separate(boids); // The three flocking rules
        Vector2 ali = Align(boids);
        Vector2 coh = Cohesion(boids);

        sep *= 3.0f; // Arbitrary weights for these forces (Try different ones!)
        ali *= 1.5f;
        coh *= 0.5f;

        ApplyForce(sep); // Applying all the forces
        ApplyForce(ali);
        ApplyForce(coh);

        checkBounds(); // To loop the world to the other side of the screen.
    }

    public Vector2 Align(List<Boid> boids) 
    {
        float neighborDist = 6f; // This is an arbitrary value and could vary from boid to boid.

        /* Add up all the velocities and divide by the total to
         * calculate the average velocity. */
        Vector2 sum = Vector2.zero;
        int count = 0;
        foreach (Boid other in boids) 
        {
            float d = Vector2.Distance(location, other.location);
            if((d > 0) && (d < neighborDist))
            {
                sum += other.velocity;
                count++; // For an average, we need to keep track of how many boids are within the distance.
            }
        }

        if (count > 0)
        {
            sum /= count;

            sum = sum.normalized * maxSpeed; // We desite to go in that direction at maximum speed.

            Vector2 steer = sum - velocity; // Reynolds's steering force formula.
            steer = Vector2.ClampMagnitude(steer, maxForce);
            return steer;
        }
        else 
        {
            return Vector2.zero; // If we don't find any close boids, the steering force is Zero.
        }
    }

    public Vector2 Cohesion(List<Boid> boids)
    {
        float neighborDist = 6f;
        Vector2 sum = Vector2.zero;
        int count = 0;
        foreach (Boid other in boids)
        {
            float d = Vector2.Distance(location, other.location);
            if ((d > 0) && (d < neighborDist))
            {
                sum += other.location; // Adding up all the other's locations
                count++;
            }
        }
        if (count > 0)
        {
            sum /= count;
            /* Here we make use of the Seek() function we wrote in
             * Example 6.8. The target we seek is thr average
             * location of our neighbors. */
            return Seek(sum);
        }
        else
        {
            return Vector2.zero;
        }
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

    public Vector2 Separate(List<Boid> boids)
    {
        Vector2 sum = Vector2.zero;
        int count = 0;

        float desiredSeperation = myVehicle.transform.localScale.x * 2;

        foreach (Boid other in boids)
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