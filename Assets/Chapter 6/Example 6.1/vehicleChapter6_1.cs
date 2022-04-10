using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleChapter6_1 : MonoBehaviour
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
        //assign the mover's GameObject to the variable
        vehicle = this.gameObject;
        body = vehicle.AddComponent<Rigidbody>();
        
        maxspeed = 4.0f;
        maxforce = 1f;

        r = 1.0f;
        mass = (4 / 3) * Mathf.PI * (Mathf.Pow(r, 3));

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

    public void Seek(Vector3 target)
    {
        Vector3 desired = target - body.transform.position;
        desired.Normalize();
        desired *= maxspeed;
        Vector3 steer = desired - body.velocity;
        Debug.Log(desired);
        steer.x = Mathf.Clamp(steer.x, -maxforce, maxforce);
        steer.y = Mathf.Clamp(steer.y, -maxforce, maxforce);
        steer.z = Mathf.Clamp(steer.z, -maxforce, maxforce);
        ApplyForce(steer);
    }

    //Newton's second law
    //Receive a force, divide by mass, and add to acceleration
    public void ApplyForce(Vector3 force)
    {
        body.AddForce(force * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
