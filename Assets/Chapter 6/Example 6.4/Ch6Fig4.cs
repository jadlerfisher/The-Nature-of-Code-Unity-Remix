using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch6Fig4 : MonoBehaviour
{
    private Ch6Fig4Vehicle vehicle;
    private Ch6Fig4FlowField field;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = new Ch6Fig4Vehicle();
        field = new Ch6Fig4FlowField();
    }

    // Update is called once per frame
    void Update()
    {
        vehicle.Follow(field);
        vehicle.Update();
    }
}

public class Ch6Fig4Vehicle
{
    private GameObject vehicleRepresentation;
    private Vector2 location;
    private Vector2 velocity;
    private Vector2 acceleration;
    private float maxSpeed;
    private float maxForce;
    private Vector2 minimumPos;
    private Vector2 maximumPos;

    public Ch6Fig4Vehicle()
    {
        vehicleRepresentation = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        location = Vector2.zero;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        maxSpeed = 4f;
        maxForce = 0.1f;
        findWindowBounds();
    }

    public void Follow(Ch6Fig4FlowField flow)
    {        
        Vector2 desiredVelocity = flow.Lookup(location);
        desiredVelocity *= maxSpeed;        
        Vector2 steerVelocity = desiredVelocity - velocity;
        Vector2.ClampMagnitude(steerVelocity, maxForce);
        applyForce(steerVelocity);
    }

    private void applyForce(Vector2 forceToAdd)
    {
        acceleration += forceToAdd;
    }

    public void Update()
    {
        velocity += acceleration * Time.fixedDeltaTime;
        Vector2.ClampMagnitude(velocity, maxSpeed);
        location += velocity * Time.fixedDeltaTime;
        acceleration *= 0;
        display();
        stayWithinScreen();
    }

    private void stayWithinScreen()
    {
        if (location.x > maximumPos.x || location.x < minimumPos.x
         || location.y > maximumPos.y || location.y < minimumPos.y)
        {            
            location = vehicleRepresentation.transform.position = Vector2.zero;            
        }
    }

    private void display()
    {
        vehicleRepresentation.transform.position = location;
    }

    private void findWindowBounds()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        minimumPos = -maximumPos;
    }
}

public class Ch6Fig4FlowField
{
    private Vector2[,] field;
    private int columns, rows;
    private int resolution;

    public Ch6Fig4FlowField()
    {
        resolution = 10;
        columns = Screen.width / resolution;        
        rows = Screen.height / resolution;
        field = new Vector2[columns, rows];
        initializeFlowField();        
    }

    private void initializeFlowField()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++) 
            {
                Vector2 v = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f,1f));
                v.Normalize();                
                field[i,j] = v;
            }
        }
    }

    public Vector2 Lookup(Vector2 _lookUp)
    {
        int column = (int)Mathf.Clamp(_lookUp.x, 0, columns - 1);
        int row = (int)Mathf.Clamp(_lookUp.y, 0, rows - 1);
        return field[column, row];
    }
}
