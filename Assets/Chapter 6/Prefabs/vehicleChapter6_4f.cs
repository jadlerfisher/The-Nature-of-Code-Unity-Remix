using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehicleChapter6_4f : MonoBehaviour
{
    public Vector3 location;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 wanderAngle;

    public float r;
    public float maxforce;
    public float maxspeed;
    public float mass;

    private GameObject vehicle;
    private Vector3 circleCenter;

    // Start is called before the first frame update
    void Start()
    {
        //assign the mover's GameObject to the varaible
        vehicle = this.gameObject;
        location = this.gameObject.transform.position;
        r = 3.0f;
        maxspeed = 10f;
        maxforce = 10f;
        mass = 1000f;

        //Assign that spawn location to the mover
        vehicle.transform.position = location;
        acceleration = new Vector3(0f, 0f, 0f);
        velocity = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //velocity += new Vector3(acceleration.x, acceleration.y, acceleration.z);

        //velocity.x = Mathf.Clamp(velocity.x, -maxspeed, maxspeed);
        //velocity.y = Mathf.Clamp(velocity.y, -maxspeed, maxspeed);
        //velocity.z = Mathf.Clamp(velocity.z, -maxspeed, maxspeed);
        location += new Vector3(acceleration.x, acceleration.y, acceleration.z);

        this.gameObject.transform.position = location;

        acceleration *= 0;
        //We need to update the wander angle each frame to steer the vehicle
        wanderAngle = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
    }

    public void seek(Vector3 target)
    {
        location = this.gameObject.transform.position;
        Vector3 desired = target - location;
        desired.Normalize();
        desired *= maxspeed;
        Vector3 steer = desired - velocity;
        Debug.Log(desired);
        steer.x = Mathf.Clamp(steer.x, -maxforce, maxforce);
        steer.y = Mathf.Clamp(steer.y, -maxforce, maxforce);
        steer.z = Mathf.Clamp(steer.z, -maxforce, maxforce);
        applyForce(steer);
    }

    public void arrive(Vector3 target)
    {
        location = this.gameObject.transform.position;
        Vector3 desired = target - location;
        float d = desired.magnitude;
        Debug.Log(d);
        if (d < 1)
        {
            float m = ExtensionMethods.Map(d, 0f, 1f, 0, maxspeed);
            desired *= m;

        } else
        {
            desired *= maxspeed;
        }

        Vector3 steer = desired - velocity;
        Debug.Log(desired);
        steer.x = Mathf.Clamp(steer.x, -maxforce, maxforce);
        steer.y = Mathf.Clamp(steer.y, -maxforce, maxforce);
        steer.z = Mathf.Clamp(steer.z, -maxforce, maxforce);
        applyForce(steer);
    }

    public void wander()
    {
        circleCenter = velocity;
        Vector3.Normalize(circleCenter);
        float CircleDistance = 2f;
        circleCenter *= CircleDistance;

        Vector3 displacement = new Vector3(0f, 1f, 0f);
        float CircleRadius = 2f;
        displacement *= CircleRadius;

        float AngleChange = Vector3.Angle(displacement, wanderAngle);
        wanderAngle.x += (Random.value * AngleChange) - (AngleChange * .5f);
        wanderAngle.y += (Random.value * AngleChange) - (AngleChange * .5f);
        wanderAngle.z += (Random.value * AngleChange) - (AngleChange * .5f);

        Vector3 wanderForce = circleCenter + displacement;
        wanderForce.x = Mathf.Clamp(wanderAngle.x, -maxforce, maxforce);
        wanderForce.y = Mathf.Clamp(wanderAngle.y, -maxforce, maxforce);
        wanderForce.z = Mathf.Clamp(wanderAngle.z, -maxforce, maxforce);
        applyForce(wanderForce);
    }

    public void follow(flowFieldChapter6_4 flow)
    {
        Vector3 desiredVector = flow.lookup(location);
        Vector3 desired = new Vector3(desiredVector.x, desiredVector.y, 0);
        desired *= maxspeed;

        Vector3 steer = desired - velocity;
        steer.x = Mathf.Clamp(steer.x, -maxforce, maxforce);
        steer.y = Mathf.Clamp(steer.y, -maxforce, maxforce);
        steer.z = Mathf.Clamp(steer.z, -maxforce, maxforce);
        applyForce(steer*10);
    }

    //Newton's second law
    //Receive a force, divide by mass, and add to acceleration
    public void applyForce(Vector3 force)
    {
        Vector3 f = force / mass;
        acceleration = acceleration + f;
    }
}