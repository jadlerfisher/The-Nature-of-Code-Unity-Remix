using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehicleChapter6_7 : MonoBehaviour
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

    private float worldRecord = 1000000f;


    // Start is called before the first frame update
    void Start()
    {
        //assign the mover's GameObject to the varaible
        vehicle = this.gameObject;
        location = this.gameObject.transform.position;
        r = 3.0f;
        maxspeed = 1f;
        maxforce = 10f;
        mass = 10000f;

        //Assign that spawn location to the mover
        vehicle.transform.position = location;
        acceleration = new Vector3(0f, 0f, 0f);
        velocity = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity += new Vector3(acceleration.x, acceleration.y, acceleration.z);

        velocity.x = Mathf.Clamp(velocity.x, -maxspeed, maxspeed);
        velocity.y = Mathf.Clamp(velocity.y, -maxspeed, maxspeed);
        velocity.z = Mathf.Clamp(velocity.z, -maxspeed, maxspeed);
        location += new Vector3(velocity.x, velocity.y, velocity.z);

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
            float m = ExtensionMethods.map(d, 0f, 1f, 0, maxspeed);
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

    public void pathFollow(Path6_6 path) {

        float distance = new float();

        Vector3 predict = velocity;
        Vector3 pNormal = predict.normalized;
        pNormal *= 20;
        Vector3 predictedLocation = location + pNormal;
        /*
        for (int i = 0; i < path.pathVectors.Count - 1; i++)
        {           
            Vector3 a = path.pathVectors[i];
            Vector3 b = path.pathVectors[i + 1];

            Vector3 normalPoint = getNormalPoint(predictedLocation, a, b);
            if(normalPoint.x < a.x || normalPoint.x > b.x)
            {
                normalPoint = b;
            }

            distance = Vector3.Distance(normalPoint, predictedLocation);

            if (distance > 10)
            {
                Vector3 target = normalPoint;
                seek(target);
            }  
            else
            {
                wander();
            }
        }
        */

    }


    public void separate(List<GameObject> vehicles)
    {
        Vector3 sum = new Vector3();
        int count = 0;
        float desiredSeparation = r*2;

        foreach(GameObject other in vehicles)
        {
            float d = Vector3.Distance(location, other.GetComponent<vehicleChapter6_7>().location);
  
            if ((d > 0) && (d < desiredSeparation))
            {
                Vector3 diff = location - other.GetComponent<vehicleChapter6_7>().location;
                Vector3 diffNormal = diff.normalized;
                sum += diff;
                count++;

            }
        }

        if (count > 0)
        {
            sum /= count;
            Vector3 sumN = sum.normalized;
            sumN *= maxspeed;
            Vector3 steer = sum - velocity;
            steer.x = Mathf.Clamp(steer.x, -maxforce, maxforce);
            steer.y = Mathf.Clamp(steer.y, -maxforce, maxforce);
            steer.z = Mathf.Clamp(steer.z, -maxforce, maxforce);
            applyForce(steer);
        }
    }

    //Newton's second law
    //Receive a force, divide by mass, and add to acceleration
    public void applyForce(Vector3 force)
    {
        Vector3 f = force / mass;
        acceleration = acceleration + f;
    }



    Vector3 getNormalPoint(Vector3 p, Vector3 a, Vector3 b)
    {
        Vector3 ap = p - a;
        Vector3 ab = b - a;

        Vector3 abNormal = ab.normalized;

        abNormal *= (Vector3.Dot(ap,abNormal));

        Vector3 normalPoint = a + abNormal;

        return normalPoint;

    }
}