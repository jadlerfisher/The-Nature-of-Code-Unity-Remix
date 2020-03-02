using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig1 : MonoBehaviour
{

    //variables for the speed of mover
    private float xspeed = .03F;
    private float yspeed = .03F;
    private float zspeed = .03F;

    // Set the borders of our exercise so we keep the mover bouncing in a single space
    private float xMin = -10, xMax = 10, yMin = -10, yMax = 10, zMin = -10, zMax = 10;
    public bool xHit, yHit, zHit = true;

    // Variables for the mover and its location coordinates
    public GameObject mover;
    private float x;
    private float y;
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate our mover
        mover = Instantiate(mover, new Vector3(0, 0, 0), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {

        //Grab the transform variables of the mover so we can see when the its hits the borders of our exercise
        x = mover.transform.position.x;
        y = mover.transform.position.y;
        z = mover.transform.position.z;

        //Each frame, check to see whether the mover's x,y, or z position coordinates have HIT a border 
        //and if so, to either add a value (+=) or substract a valur (-=) from the vector
        if (xHit)
        {
            mover.transform.position += new Vector3(xspeed, 0, 0);
            if (x > xMax)
            {
                xHit = false;
            }
        }
        else
        {
            mover.transform.position -= new Vector3(xspeed, 0, 0);
            if (x < xMin)
            {
                xHit = true;
            }
        }
        if (yHit)
        {
            mover.transform.position += new Vector3(0, yspeed, 0);
            if (y > yMax)
            {
                yHit = false;
            }
        }
        else
        {
            mover.transform.position -= new Vector3(0, yspeed, 0);
            if (y < yMin)
            {
                yHit = true;
            }
        }
        if (zHit)
        {
            mover.transform.position += new Vector3(0, 0, zspeed);
            if (z > zMax)
            {
                zHit = false;
            }
        }
        else
        {
            mover.transform.position -= new Vector3(0, 0, zspeed);
            if (z < zMin)
            {
                zHit = true;
            }
        }
    }
}