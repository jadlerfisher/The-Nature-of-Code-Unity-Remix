using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig3 : MonoBehaviour
{
    Mover mover;
    //Mouse coordinates
    Vector2 inWorldMousePosition;

    float turnSpeed = 10f;
    float moveSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        mover = new Mover();
        inWorldMousePosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        inWorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float gotoX = inWorldMousePosition.x - mover.location.x;
        float gotoY = inWorldMousePosition.y - mover.location.y;

        float angle = Mathf.Atan2(gotoX, gotoY);

        mover.Translate(moveSpeed);
        mover.Rotate(angle, turnSpeed);
        mover.Update();
    }
}

public class Mover
{
    // The basic properties of a mover class
    public Vector2 location, velocity, acceleration;
    public float mass;

    private Vector2 maximumPos;

    private GameObject mover;

    public Mover()
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.black;

        mover.transform.localScale = new Vector3(0.5f, 1, 0.5f);

        mass = 1;

        location = Vector2.zero;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;

        FindWindowLimits();
    }

    public void Translate(float speed)
    {
        velocity = (Vector2) mover.transform.up * speed;
    }

    public void Rotate(float radiansAngle, float turnSpeed) 
    {
        float eulerAngle = (-radiansAngle * Mathf.Rad2Deg) + 180;
        float toSpin = eulerAngle - ((mover.transform.eulerAngles.z + 180) % 360);
        if (toSpin > 180 || toSpin < -180) {
            toSpin %= 180;
            toSpin *= -1;
        }
        
        toSpin = Mathf.Clamp(toSpin, -turnSpeed, turnSpeed);
        mover.transform.Rotate(new Vector3(0, 0, toSpin));
    }

    public void Update()
    {
        velocity += acceleration * Time.deltaTime;
        location += velocity * Time.deltaTime;

        acceleration = Vector2.zero;

        mover.transform.position = location;

        CheckEdges();
    }

    public void CheckEdges()
    {
        if (location.x > maximumPos.x)
        {
            location.x = -maximumPos.x;
        }
        else if(location.x < -maximumPos.x)
        {
            location.x = maximumPos.x;
        }
        if (location.y > maximumPos.y)
        {
            location.y = -maximumPos.y;
        } 
        else if (location.y < -maximumPos.y) 
        {
            location.y = maximumPos.y;
        }
    }

    private void FindWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 8;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
