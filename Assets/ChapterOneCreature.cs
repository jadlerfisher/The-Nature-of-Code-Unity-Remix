using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterOneCreature : MonoBehaviour
{
    Vector3 location;
    Vector3 acceleration;
    Vector3 velocity;

    float topSpeed;

    public float minX, minY, minZ, maxX, maxY, maxZ;

    // Start is called before the first frame update
    void Start()
    {
        minX = 5f;
        maxX = 115f;

        minZ = 5f;
        maxZ = 115f;

        minY = 2f;
        maxY = 30f;


        location = this.gameObject.transform.position;
        velocity = Vector3.zero;
        acceleration = new Vector3(-0.1F, 0f, -1F);
        topSpeed = 10F;
    }

    // Update is called once per frame
    void Update()
    {
        // Speeds up the mover
        velocity += acceleration * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

        // Limit Velocity to the top speed
        velocity = Vector3.ClampMagnitude(velocity, topSpeed);

        // Moves the mover
        location += velocity * Time.deltaTime;

        checkEdges();

        // Updates the GameObject of this movement
        this.gameObject.transform.position = new Vector3(location.x, location.y, location.z);
    }


    void checkEdges()
    {
        if (location.x > maxX)
        {
            location.x -= maxX - minX;
        }
        else if (location.x < minX)
        {
            location.x += maxX - minX;
        }
        if (location.y > maxY)
        {
            location.y -= maxY - minY;
        }
        else if (location.y < minY)
        {
            location.y += maxY - minY;
        }
        if (location.z > maxZ)
        {
            location.z -= maxZ - minZ;
        }
        else if (location.z < minZ)
        {
            location.z += maxZ - minZ;
        }
    }
}




//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Chapter1Fig8 : MonoBehaviour
//{
//    // Declare a mover object
//    private Mover1_8 mover;

//    // Start is called before the first frame update
//    void Start()
//    {
//        // Create a Mover object
//        mover = new Mover1_8();
//    }

//    // Update is called once per frame forever and ever (until you quit).
//    void Update()
//    {
//        mover.Update();
//        mover.CheckEdges();
//    }
//}

//public class Mover1_8
//{
//    // The basic properties of a mover class
//    private Vector2 location, velocity, acceleration;
//    private float topSpeed;

//    // The window limits
//    private Vector2 minimumPos, maximumPos;


//    // Gives the class a GameObject to draw on the screen
//    private GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

//    public Mover1_8()
//    {
//        findWindowLimits();
//        location = Vector2.zero; // Vector2.zero is a (0, 0) vector
//        velocity = Vector2.zero;
//        acceleration = new Vector2(-0.1F, -1F);
//        topSpeed = 10F;

//        //We need to create a new material for WebGL
//        Renderer r = mover.GetComponent<Renderer>();
//        r.material = new Material(Shader.Find("Diffuse"));
//    }

//    public void Update()
//    {
//        // Speeds up the mover
//        velocity += acceleration * Time.deltaTime; // Time.deltaTime is the time passed since the last frame.

//        // Limit Velocity to the top speed
//        velocity = Vector2.ClampMagnitude(velocity, topSpeed);

//        // Moves the mover
//        location += velocity * Time.deltaTime;


//        // Updates the GameObject of this movement
//        mover.transform.position = new Vector2(location.x, location.y);
//    }

//    public void CheckEdges()
//    {
//        if (location.x > maximumPos.x)
//        {
//            location.x -= maximumPos.x - minimumPos.x;
//        }
//        else if (location.x < minimumPos.x)
//        {
//            location.x += maximumPos.x - minimumPos.x;
//        }
//        if (location.y > maximumPos.y)
//        {
//            location.y -= maximumPos.y - minimumPos.y;
//        }
//        else if (location.y < minimumPos.y)
//        {
//            location.y += maximumPos.y - minimumPos.y;
//        }
//    }

//    private void findWindowLimits()
//    {
//        // The code to find the information on the camera as seen in Figure 1.2

//        // We want to start by setting the camera's projection to Orthographic mode
//        Camera.main.orthographic = true;
//        // Next we grab the minimum and maximum position for the screen
//        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
//        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
//    }
//}