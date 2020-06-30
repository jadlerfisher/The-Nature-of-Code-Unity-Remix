using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehicleChapter6_1 : MonoBehaviour
{
    public float r;
    public float maxforce;
    public float maxspeed;
    public float mass;

    private GameObject vehicle;
    public GameObject target;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        //assign the mover's GameObject to the variable
        vehicle = this.gameObject;
        body = vehicle.AddComponent<Rigidbody>();
        
        r = 3.0f;
        maxspeed = 4.0f;
        maxforce = 1f;

        body.drag = 0;
        body.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        body.velocity = new Vector3(
            Mathf.Clamp(body.velocity.x, -maxspeed, maxspeed),
            Mathf.Clamp(body.velocity.y, -maxspeed, maxspeed),
            Mathf.Clamp(body.velocity.z, -maxspeed, maxspeed));

        //arrive(target.transform.position);
        vehicle.transform.rotation = Quaternion.LookRotation(body.angularVelocity);
    }


    public void seek(Vector3 target)
    {
        Vector3 desired = target - body.transform.position;
        desired.Normalize();
        desired *= maxspeed;
        Vector3 steer = desired - body.velocity;
        Debug.Log(desired);
        steer.x = Mathf.Clamp(steer.x, -maxforce, maxforce);
        steer.y = Mathf.Clamp(steer.y, -maxforce, maxforce);
        steer.z = Mathf.Clamp(steer.z, -maxforce, maxforce);
        applyForce(steer);
    }

    //Newton's second law
    //Receive a force, divide by mass, and add to acceleration
    public void applyForce(Vector3 force)
    {
        body.AddForce(force * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
