using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverChapter3_2 : MonoBehaviour
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

    private Vector3 exerciseBounds;

    public Vector3 aVelocity = new Vector3(0f, 0f, 0f);
    public Vector3 aAcceleration = new Vector3(0f, 0f, .1f);
    public Vector3 angle = new Vector3(0f, 0f, 0f); 

    // Start is called before the first frame update
    void Awake()
    {

        //assign the mover's GameObject to the varaible
        mover = this.gameObject;

        //Assign that spawn location to the mover
        mover.transform.position = location;
        acceleration = new Vector3(0f, 0f, 0f);
        mass = 100f;


        //Create boundaries
        exerciseBounds = Camera.main.WorldToViewportPoint(transform.position);
        exerciseBounds.x = Mathf.Clamp01(exerciseBounds.x);
        exerciseBounds.y = Mathf.Clamp01(exerciseBounds.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mover.transform.position = location;

        if ((location.x / 10) <= exerciseBounds.x && (location.y / 10) <= exerciseBounds.y)
        {

        
                //add the value of acceleration each frame to the mover's velocity
                velocity += new Vector3(acceleration.x, acceleration.y, acceleration.z);
                //add that velocity value to the transform of the mover's position
                location += new Vector3(velocity.x, velocity.y, velocity.z);

            aAcceleration.x = acceleration.x / 10f;
            aVelocity.x += aAcceleration.x;
            aVelocity.x = Mathf.Clamp(aVelocity.x, -.1f, .1f);
            angle += aVelocity;

            mover.transform.Rotate(angle, Space.World);
            multiplyVector(acceleration, 0);








        }
        else if ((location.x / 10) >= exerciseBounds.x)
        {

            velocity.x *= -1;
            location.x = exerciseBounds.x * 9;



        }
        else if ((location.x / 10) < 0)
        {

            velocity.x *= -1;
            location.x = 0f;
        }
        else if ((location.y / 10) > exerciseBounds.y)
        {

            velocity.y *= -1;
            location.y = exerciseBounds.y * 9;


        }
        else if ((location.y / 10) < 0)
        {

            velocity.y *= -1;
            location.y = 0;


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

    //Newton's second law
    //Receive a force, divide by mass, and add to acceleration
    public void applyForce(Vector3 force)
    {
        Vector3 f = force / mass;
        acceleration = acceleration + f;
    }

    //Law of Attraction
    public void attract(GameObject m)
    {

        Vector3 difference = m.transform.position - this.transform.position;
        float dist = difference.magnitude;
         Vector3 gravityDirection = difference.normalized;
         float gravity = 6.7f * (m.transform.localScale.x * this.transform.localScale.x) / (dist * dist);
        Vector3 gravityVector = (gravityDirection * gravity);
        applyForce(gravityVector);

    }


}
