using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bear : MonoBehaviour
{
    Vector3 location;
    Vector3 velocity;
    Vector3 acceleration;
    Vector3 topSpeed = new Vector3(6f, 6f, 6f);

    Rigidbody m_Rigidbody;

    void Start()
    {
        location = this.gameObject.transform.position;
        velocity = new Vector3(0F, 0F, 0F);
        acceleration = new Vector3(.0F, .0F, .0F);

        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (velocity.magnitude <= topSpeed.magnitude)
        {
            acceleration = new Vector3(Random.Range(-.041F, .041F), Random.Range(-.031F, .031F), Random.Range(-.021F, .021F));
            acceleration.Normalize();
            acceleration *= .003f;


            // Add the value of acceleration each frame to the mover's velocity
            velocity += new Vector3(acceleration.x, acceleration.y, acceleration.z);
            // Add that velocity value to the transform of the mover's position
            this.gameObject.transform.position += new Vector3(velocity.x, velocity.y, velocity.z);

            location += new Vector3(velocity.x, velocity.y, velocity.z);
            //Assign that value to the mover's gameobject
            this.gameObject.transform.LookAt(location);

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Ouput the Collision to the console
        if(collision.gameObject.name == "bear(Clone)")
        {
            Debug.Log("eh?");
            collision.gameObject.transform.GetComponent<Rigidbody>().AddForce(new Vector3 (10f,0f,10f));

        }

    }

}
