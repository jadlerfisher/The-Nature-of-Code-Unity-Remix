using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyAnimal : MonoBehaviour
{

    Vector3 location;
    Vector3 velocity;
    Vector3 acceleration;
    Vector3 topSpeed = new Vector3(2f, 2f, 2f);

    // Start is called before the first frame update
    void Start()
    {
        location = this.gameObject.transform.position;
        velocity = new Vector3(0F, 0F, 0F);
        acceleration = new Vector3(.0F, .0F, .0F);
    }

    // Update is called once per frame
    void Update()
    {
        if (velocity.magnitude <= topSpeed.magnitude)
        {
            acceleration = new Vector3(Random.Range(-.001F, .001F), Random.Range(-.001F, .001F), Random.Range(-.001F, .001F));
            acceleration.Normalize();
            acceleration *= .001f;
            // Add the value of acceleration each frame to the mover's velocity
            velocity += new Vector3(acceleration.x, acceleration.y, acceleration.z);
            // Add that velocity value to the transform of the mover's position
            location += new Vector3(velocity.x, velocity.y, velocity.z);
            //Assign that value to the mover's gameobject
            this.gameObject.transform.LookAt(location);
            this.gameObject.transform.position = location;
            acceleration *= .0001f;

        }
        else
        {
            velocity += new Vector3(acceleration.x, acceleration.y, acceleration.z);
            location += new Vector3(velocity.x, velocity.y, velocity.z);
            this.gameObject.transform.LookAt(location);
            this.gameObject.transform.position = location;
            acceleration *= 0f;
        }

        if (location.x >= 100f && location.x <110)
        {

            velocity.x *= -1f;
        }
        else if (location.x <= 0)
        {
            velocity.x *= -1f; 
        }
        else if (location.y >= 6f)
        {
            velocity.y *= -.01f;
        }
        else if (location.y <= 0)
        {
            velocity.y *= -.01f;
        }
        else if (location.z <= 0)
        {
            velocity.z *= -1f;
        }
        else if (location.z >= 100f && location.z < 110f)
        {
            velocity.z *= -1f;
        }


        if (location.z >= 110f || location.x >= 110f)
        {
            location = new Vector3(Random.Range(1f, 100f), Random.Range(2f, 6f), Random.Range(1f, 100f));
            velocity = new Vector3(0f, 0f, 0f);
            acceleration = new Vector3(Random.Range(-.001F, .001F), Random.Range(-.001F, .001F), Random.Range(-.001F, .001F));

        }

    }


}
