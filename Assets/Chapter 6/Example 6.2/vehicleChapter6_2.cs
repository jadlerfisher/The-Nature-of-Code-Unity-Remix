using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehicleChapter6_2 : MonoBehaviour
{
    public Vector3 location;
    public Vector3 velocity;
    public Vector3 acceleration;

    public float r;
    public float maxforce;
    public float maxspeed;
    public float mass;

    private GameObject vehicle;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        //assign the mover's GameObject to the varaible
        vehicle = this.gameObject;
        location = this.gameObject.transform.position;
        r = 3.0f;
        maxspeed = 4.0f;
        maxforce = 1f;
        mass = 1000f;

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
        acceleration *= 0;
        this.gameObject.transform.position = location;

        //arrive(target.transform.position);
        vehicle.transform.rotation = Quaternion.LookRotation(velocity);
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
        //desired = desired.normalized;
        Debug.Log(d);
        if (d < 100)
        {
            desired = Vector3.ClampMagnitude(desired * d, maxspeed);
            //float m = ExtensionMethods.Remap(d, 0f, 1f, 0, maxspeed);
            //desired *= m;
            Debug.Log("near" + desired);

        } else
        {
            desired *= maxspeed;
            Debug.Log("far" + desired);
        }

        Vector3 steer = desired - velocity;
        //  Debug.Log(desired);
        steer.x = Mathf.Clamp(steer.x, -maxforce, maxforce);
        steer.y = Mathf.Clamp(steer.y, -maxforce, maxforce);
        steer.z = Mathf.Clamp(steer.z, -maxforce, maxforce);
        applyForce(steer);
        //Debug.Log(steer);
    }

    //Newton's second law
    //Receive a force, divide by mass, and add to acceleration
    public void applyForce(Vector3 force)
    {
        Vector3 f = force / mass;
        acceleration = acceleration + f;
    }
}