using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig3 : MonoBehaviour
{
    private Ch6Fig3Vehicle vehicle;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = new Ch6Fig3Vehicle(0,0);
    }

    // FixedUpdate is called once per Physics Update which is 50 times a second unless changed in Project Settings
    void FixedUpdate()
    {
        vehicle.Update();
        vehicle.StayWithinWalls();
    }
}

public class Ch6Fig3Vehicle
{
    private GameObject vehicleObject;    
    private Vector2 location;
    private Vector2 velocity;
    private Vector2 acceleration;    
    private float maxSpeed;
    private float maxForce;
    private Vector2 maximumPos;
    private Vector2 minimumPos;
    private float distance; // TODO expose this as a range

    public Ch6Fig3Vehicle(float locationX, float locationY)
    {
        findWindowLimits();
        vehicleObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);        
        Object.Destroy(vehicleObject.GetComponent<SphereCollider>());
        Renderer renderer = vehicleObject.GetComponent<Renderer>();        
        renderer.material = new Material(Shader.Find("Diffuse"));
        location = new Vector2(locationX, locationY);
        vehicleObject.transform.position = location;
        velocity = Vector2.zero;
        acceleration = new Vector2(-150,150); // TODO expose these
        maxSpeed = 4f;
        maxForce = 0.1f;
    }

    public void Update()
    {
        velocity += acceleration * Time.fixedDeltaTime;
        Vector2.ClampMagnitude(velocity, maxSpeed);
        location += velocity * Time.fixedDeltaTime;
        vehicleObject.transform.position = location;
        acceleration *= 0;
        Debug.Log(velocity);
    }

    public void StayWithinWalls() 
    {
        if (location.x < minimumPos.x + distance)
        {
            Vector2 desired = new Vector2(maxSpeed, velocity.y);
            Vector2 steer = desired - velocity;
            Vector2.ClampMagnitude(steer, maxForce);
            applyForce(steer);
        }
        else if (location.x > maximumPos.x - distance)
        {
            Vector2 desired = new Vector2(-maxSpeed, velocity.y);
            Vector2 steer = desired - velocity;
            Vector2.ClampMagnitude(steer, maxForce);
            applyForce(steer);
        }

        if (location.y < minimumPos.y + distance)
        {
            Vector2 desired = new Vector2(velocity.x, maxSpeed);
            Vector2 steer = desired - velocity;
            Vector2.ClampMagnitude(steer, maxForce);
            applyForce(steer);
        }
        else if (location.y > maximumPos.y - distance)
        {
            Vector2 desired = new Vector2(velocity.x, -maxSpeed);
            Vector2 steer = desired - velocity;
            Vector2.ClampMagnitude(steer, maxForce);
            applyForce(steer);
        }
    }
    
    private void applyForce(Vector2 force)
    {
        acceleration += force;
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        minimumPos = -maximumPos;
        distance = 3f;
    }
}
