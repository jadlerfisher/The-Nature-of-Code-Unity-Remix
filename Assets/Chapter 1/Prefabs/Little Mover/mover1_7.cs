using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mover1_7 : MonoBehaviour
{

    // Create your speed variables for the mover class
    public Vector3 location;
    public Vector3 velocity;

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
        velocity = new Vector3(Random.Range(-.1F, .1F), Random.Range(-.1F, .1F), Random.Range(-.1F, .1F));

        //Assign that spawn location to the mover
        mover.transform.position = location;
    }


    // Update is called once per frame
    void Update()
    {
        //Check to make sure we are inside the border
        if ((location.x >= xMin) && (location.x <= xMax) && (location.y >= yMin) && (location.y <= yMax) && (location.z >= zMin) && (location.z <= zMax))
        {
        // Add the velocity value to the transform of the mover's position
        location += new Vector3(velocity.x, velocity.y, velocity.z);
            //Assign that value to the mover's gameobject
            mover.transform.Translate(location * Time.deltaTime, Space.World);
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

        //Each frame, check to see whether the ball's x,y, or z position coordinates have HIT a border and if so, 
        //Go back to the origin.
        if (xHit)
        {
            location = new Vector3(0F, 0F, 0F);
        }
        else if (yHit)
        {
            location = new Vector3(0F, 0F, 0F);
        }
        else if (zHit)
        {
            location = new Vector3(0F, 0F, 0F);
        }
    }
}
