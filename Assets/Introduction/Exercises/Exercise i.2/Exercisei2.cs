using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercisei2 : MonoBehaviour
{
    //We need to create a walker
    exerciseIntro2Mover walker;

    //Another moving object that the walker will follow
    movingObject moveObj;

    // Start is called before the first frame update
    void Start()
    {
        moveObj = new movingObject();
        walker = new exerciseIntro2Mover(moveObj);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Have the walker choose a direction
        walker.step();
        walker.CheckEdges();

        //Moving Object has to move
        moveObj.step();
        moveObj.CheckEdges();
    }
}


public class movingObject
{
    private Vector3 location;

    private Vector2 minimumPos, maximumPos;

    public GameObject movingObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

    public movingObject()
    {
        findWindowLimits();
        location = new Vector2(1f, 1f);
        //We need to create a new material for WebGL
        Renderer r = movingObj.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void step()
    {
        location = movingObj.transform.position;
        //Each frame choose a new Random number 0,1,2,3, 
        //If the number is equal to one of those values, take a step
        int choice = Random.Range(0, 4);
        if (choice == 0)
        {
            location.x++;
        }
        else if (choice == 1)
        {
            location.x--;
        }
        else if (choice == 3)
        {
            location.y++;
        }
        else
        {
            location.y--;
        }

        movingObj.transform.position += location * Time.deltaTime;
    }

    public void CheckEdges()
    {
        location = movingObj.transform.position;

        if (location.x > maximumPos.x)
        {
            location = Vector2.zero;
        }
        else if (location.x < minimumPos.x)
        {
            location = Vector2.zero;
        }
        if (location.y > maximumPos.y)
        {
            location = Vector2.zero;
        }
        else if (location.y < minimumPos.y)
        {
            location = Vector2.zero;
        }
        movingObj.transform.position = location;
    }

    private void findWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}


public class exerciseIntro2Mover
{
    // The basic properties of a mover class
    // x, y, z 
    private Vector3 location;

    // The window limits
    private Vector2 minimumPos, maximumPos;

    // Gives the class a GameObject to draw on the screen
    public GameObject mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    //Moving Object 
    movingObject movingOb;

    public exerciseIntro2Mover(movingObject movingObj)
    {
        findWindowLimits();
        location = Vector2.zero;
        movingOb = movingObj;
        //We need to create a new material for WebGL
        Renderer r = mover.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
    }

    public void step()
    {
        float num = Random.Range(0f, 1f);

        location = mover.transform.position;
        //Each frame choose a new Random number 0-1;
        //If the number is less than the the float take a step
        if (num < 0.2F)
        {
            location.y++;
            mover.transform.position += location * Time.deltaTime;
        }
        else if (num > 0.2F && num < 0.4F)
        {
            location.y--;
            mover.transform.position += location * Time.deltaTime;
        }
        else if (num > 0.4F && num < .6F)
        {
            location.x--;
            mover.transform.position += location * Time.deltaTime;
        }
        else if (num > .6f)
        {
            mover.transform.position = Vector3.MoveTowards(mover.transform.position, movingOb.movingObj.transform.position, Time.deltaTime);
        }
        

    }

    public void CheckEdges()
    {
        location = mover.transform.position;

        if (location.x > maximumPos.x)
        {
            location = Vector2.zero;
        }
        else if (location.x < minimumPos.x)
        {
            location = Vector2.zero;
        }
        if (location.y > maximumPos.y)
        {
            location = Vector2.zero;
        }
        else if (location.y < minimumPos.y)
        {
            location = Vector2.zero;
        }
        mover.transform.position = location;
    }

    private void findWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;
        // Next we grab the minimum and maximum position for the screen
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
