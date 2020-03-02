using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Now that we understand how to add two vectors together, we can look at how addition is implemented in the PVector class itself. Let’s write a function called add() that takes another PVector object as its argument.

public class PVector
{

    public float x;
    public float y;
    public float z;

    public PVector(float x_, float y_, float z_)
    {
        x = x_;
        y = y_;
        z = z_;
    }

    public void add(PVector v)
    {
        x = x + v.x;
        y = y + v.y;
        z = z + v.z;

    }

}



public class Chapter1Fig2 : MonoBehaviour
{
    //Instead we'll go ahead and use the Vector3 Object from Unity so everything plays nice.
    private Vector3 location;
    private Vector3 velocity;
    
    // Variables for the mover and its position coordinates
    public GameObject mover;
    private float x;
    private float y;
    private float z;

    // Set the borders of our exercise so we keep the mover bouncing in a single space
    private float xMin = -10, xMax = 10, yMin = -10, yMax = 10, zMin = -10, zMax = 10;
    private bool xHit = false;
    private bool yHit = false;
    private bool zHit = false;

    // Start is called before the first frame update
    void Start()
    {
    //Generally, we would create and use the Vector3 class that comes with unity when dealing with Transforms and positions. 
    //Now we have our PVector class we created above and can use these vectors to bounce our mover
    //Instead we'll go ahead and use the Vector3 Object from Unity so everything plays nice.

    location = new Vector3(0F, 0F, 0F);
    velocity = new Vector3(.05F, .04F, .02F);

    // Instantiate our mover
    mover = Instantiate(mover, new Vector3 (location.x, location.y, location.z), Quaternion.identity);
    
    //Grab the transform variables of the mover so we can see when the its hits the borders of our exercise
    x = mover.transform.position.x;
    y = mover.transform.position.y;
    z = mover.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //Each frame, the mover's transform's position, a Vector 3, will update to a new vector based on the new Vector3 created by location += velocity;
        mover.transform.position = new Vector3(location.x, location.y, location.z);

        if ((location.x >= xMin) && (location.x <= xMax) && (location.y >= yMin) && (location.y <= yMax) && (location.z >= zMin) && (location.z <= zMax))
        {
            location += velocity;
        }
        else
        {
        //Each frame, check to see whether the mover's x,y, or z position coordinates have HIT a border and if so, to either add a value (+=) or substract a value (-=) from the vector
            if (xHit)
            {
                location = new Vector3(0F, 0F, 0F);
                if (x > xMax)
                {
                    location = new Vector3(0F,0F,0F);
                    xHit = false;
                }
            }
            else
            {
                location = new Vector3(0F, 0F, 0F);
                if (x < xMin)
                { 
                    xHit = true;
                }
            }

            if (yHit)
            {
                location = new Vector3(0F, 0F, 0F);
                if (y > yMax)
                {
                    yHit = false;
                }
            }
            else
            {
                location = new Vector3(0F, 0F, 0F);
                if (y < yMin)
                {
                    yHit = true;
                }
            }

            if (zHit)
            {
                location = new Vector3(0F, 0F, 0F);
                if (z > zMax)
                {
                    zHit = false;
                }
            }
            else
            {
                location = new Vector3(0F, 0F, 0F);
                if (z < zMin)
                {
                    zHit = true;
                }
            }
        }
    }
}

