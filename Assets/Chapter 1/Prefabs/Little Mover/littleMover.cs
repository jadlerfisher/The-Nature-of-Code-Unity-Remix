using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleMover : MonoBehaviour
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
        location = new Vector3(Random.Range(-1F, 1F), Random.Range(-1F, 1F), Random.Range(-1F, 1F));

        //Assign that spawn location to the mover
        mover.transform.position = location;

    }
        // Update is called once per frame
        void Update()
    {

        if ((location.x >= xMin) && (location.x <= xMax) && (location.y >= yMin) && (location.y <= yMax) && (location.z >= zMin) && (location.z <= zMax))
        {
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
//Check the border and push the Little Mover back
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

        //Each frame, check to see whether the ball's x,y, or z position coordinates have HIT a border and if so, to either add a value (+=) or substract a value (-=) from the vector. 
        //Then we need to bounce off the wall along a particular vector.
        if (xHit)
        {
            location.x += velocity.x;
            if (x > xMax)
            {
                location.x -= velocity.x + 1f;
                xHit = false;

            }
        }
        else
        {
            location.x -= velocity.x;
            if (x < xMin)
            {
                location.x += velocity.x + 1f;
                xHit = true;

            }
        }
        if (yHit)
        {
            location.y += velocity.y;
            if (y > yMax)
            {
                location.y -= velocity.y + 1f;
                yHit = false;

            }
        }
        else
        {
            location.y -= velocity.y;
            if (y < yMin)
            {
                location.y += velocity.y + 1f;
                yHit = true;

            }
        }

        if (zHit)
        {
            location.z += velocity.z;

            if (z > zMax)
            {
                location.y += velocity.y + 1f;
                zHit = false;

            }
        }
        else
        {
            location.z -= velocity.z;
            if (z < zMin)
            {
                location.y += velocity.y + 1f;
                zHit = true;

            }
        }
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


}
