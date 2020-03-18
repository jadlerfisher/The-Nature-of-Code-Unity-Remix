using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverChapter2_6 : MonoBehaviour
{
    public Vector3 location;
    public Vector3 velocity;
    public Vector3 acceleration;

    //Create a variable to access the Mover's information
    private GameObject mover;
    //Float coordinates for our Little Mover
    public float x;
    public float y;
    public float z;


    //The Object now has mass
    public float mass;

    float c = .01f;

    public bool collided = false;

    // Start is called before the first frame update
    void Awake()
    {

        //assign the mover's GameObject to the varaible
        mover = this.gameObject;

        //Set the initial spawn location to 0,0,0
        location = new Vector3(4f, y, z);

        //Assign that spawn location to the mover
        mover.transform.position = location;
        acceleration = new Vector3(0f, 0f, 0f);
        mass = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        mover.transform.position = location;

        //add the value of acceleration each frame to the mover's velocity
        velocity += new Vector3(acceleration.x, acceleration.y, acceleration.z);
        //add that velocity value to the transform of the mover's position
        location += new Vector3(velocity.x, velocity.y, velocity.z);
        //mover.transform.Translate(location * Time.deltaTime, Space.World);
    }

    public void subtractVector(Vector3 originalV3, Vector3 v3)
    {
        // Dividing the subtraction by 100 to keep the cursor on the screen in this example
        x = originalV3.x - v3.x;
        y = originalV3.y - v3.y;
        z = originalV3.z - v3.z;

        Vector3 subtractedVector = new Vector3(x, y, z);
        subtractedVector.Normalize();

        multiplyVector(subtractedVector, .0003f);

    }


    //Adding the multiplying vector function to manage the speed
    void multiplyVector(Vector3 transformPosition, float n)
    {
        x = transformPosition.x * n;
        y = transformPosition.y * n;
        z = transformPosition.z * n;

        acceleration = new Vector3(x, y, z);
    }

    //Newton's second law
    //Receive a force, divide by mass, and add to acceleration
    public void applyForce(Vector3 force)
    {
        Vector3 f = force / mass;
        acceleration = acceleration + f;
    }


}
