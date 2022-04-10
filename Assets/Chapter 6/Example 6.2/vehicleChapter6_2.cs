using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleChapter6_2 : MonoBehaviour
{
    [SerializeField] float r;
    [SerializeField] float maxforce;
    [SerializeField] float maxspeed;
    [SerializeField] float mass;

    private GameObject vehicle;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = this.gameObject;
        body = vehicle.AddComponent<Rigidbody>();
        
        maxspeed = 1.0f;
        maxforce = 1f;

        r = 3.0f;

        body.mass = mass;
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

        vehicle.transform.rotation = Quaternion.LookRotation(body.velocity);
    }

    public void Arrive(Vector3 target)
    {
        Vector3 desired = target - body.transform.position;
        float d = desired.magnitude;
        desired = desired.normalized;
        Debug.Log(d);
        if (d < r)
        {
            float m = ExtensionMethods.Map(d, 0f, 3f, 0, maxspeed);
            desired *= m;
            Debug.Log("near" + desired);

        } else
        {
            desired *= maxspeed;
            Debug.Log("far" + desired);
        }

        Vector3 steer = desired - body.velocity;
        ApplyForce(steer);
        Debug.DrawLine(body.transform.position, steer + body.transform.position);
    }

    //Newton's second law
    //Receive a force, divide by mass, and add to acceleration
    public void ApplyForce(Vector3 force)
    {
        body.AddForce(force * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}