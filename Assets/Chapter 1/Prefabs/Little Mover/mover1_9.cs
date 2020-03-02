using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mover1_9 : MonoBehaviour
{
    // Create your speed variables for the mover class
    public Vector3 location;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 topSpeed;

    //Create a variable to access the Mover's information
    private GameObject mover;
    //Float coordinates for our Little Mover
    private float x, y, z;

    private float xMin = -10, xMax = 10, yMin = -10, yMax = 10, zMin = -10, zMax = 10;
    private bool xHit = true, yHit = true, zHit = true;

    // Start is called before the first frame update
    void Awake()
    {
        //assign the mover's GameObject to the varaible
        mover = this.gameObject;

        //Set the initial spawn location to 0,0,0
        location = new Vector3(0F, 0F, 0F);
        velocity = new Vector3(0F, 0F, 0F);
        acceleration = new Vector3(.0F, .0F, .0F);

        //Assign that spawn location to the mover
        mover.transform.position = location;
    }
    // Update is called once per frame
    void Update()
    {

        if ((location.x >= xMin) && (location.x <= xMax) && (location.y >= yMin) && (location.y <= yMax) && (location.z >= zMin) && (location.z <= zMax))
        {
            acceleration = new Vector3(Random.Range(-.001F, .001F), Random.Range(-.001F, .001F), Random.Range(-.001F, .001F));
            acceleration.Normalize();
            multiplyVector(acceleration, .003f);

            if (velocity.magnitude <= topSpeed.magnitude)
            {

                // Add the value of acceleration each frame to the mover's velocity
                velocity += new Vector3(acceleration.x, acceleration.y, acceleration.z);
                // Add that velocity value to the transform of the mover's position
                location += new Vector3(velocity.x, velocity.y, velocity.z);
                //Assign that value to the mover's gameobject
                mover.transform.position = location;
            }
            else
            {
                velocity += new Vector3(acceleration.x, acceleration.y, acceleration.z);
                location += new Vector3(velocity.x, velocity.y, velocity.z);
                mover.transform.position = location;
            }
        }
        else
        {
            checkEdges();
        }

    }

    void checkEdges()
    {
        //Get the coordinates of the moving sphere to see when it hits a border
        x = location.x;
        y = location.y;
        z = location.z;

        //Each frame, check to see whether the ball's x,y, or z position coordinates have HIT a border and if so, 
        //Go back to the origin.
        if (xHit)
        {
            location = new Vector3(0F, 0F, 0F);
            velocity = new Vector3(0F, 0F, 0F);
            acceleration = new Vector3(.0F, .0F, .0F);
        }
        else if (yHit)
        {

            location = new Vector3(0F, 0F, 0F);
            velocity = new Vector3(0F, 0F, 0F);
            acceleration = new Vector3(.0F, .0F, .0F);
        }
        else if (zHit)
        {
            location = new Vector3(0F, 0F, 0F);
            velocity = new Vector3(0F, 0F, 0F);
            acceleration = new Vector3(.0F, .0F, .0F);
        }
    }

    //Adding the multiplying vector function to manage the speed
    void multiplyVector(Vector3 transformPosition, float n)
    {
        x = transformPosition.x * n;
        y = transformPosition.y * n;
        z = transformPosition.z * n;

        acceleration = new Vector3(x, y, z);
    }
}