using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig2 : MonoBehaviour
{
    List<Mover3_2> movers = new List<Mover3_2>(); // Now we have multiple Movers!
    Attractor2_7 a;

    // Start is called before the first frame update
    void Start()
    {
        int numberOfMovers = 10;
        for (int i = 0; i < numberOfMovers; i++)
        {
            Vector2 randomLocation = new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f));
            Vector2 randomVelocity = new Vector2(Random.Range(0f, 5f), Random.Range(0f, 5f));
            Mover3_2 m = new Mover3_2(Random.Range(.4f, 1f), randomVelocity, randomLocation); //Each Mover is initialized randomly.
            movers.Add(m);
        }
        a = new Attractor2_7();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Mover3_2 m in movers)
        {
            Rigidbody body = m.body;
            Vector2 force = a.Attract(body); // Apply the attraction from the Attractor on each Mover object

            m.body.AddForce(force, ForceMode.Force);
           
            m.body.MoveRotation(m.constrainAngularMotion(force));
            m.CheckEdges();
        }
    }
}

public class Mover3_2
{
    public Rigidbody body;
    private GameObject gameObject;
    public Transform transform;

    private Vector3 angle;
    private Quaternion angleRotation;

    private Vector2 minimumPos, maximumPos;

    public Mover3_2(float randomMass, Vector2 initialVelocity, Vector2 initialPosition)
    {
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject.Destroy(gameObject.GetComponent<BoxCollider>());
        transform = gameObject.transform;
        gameObject.AddComponent<Rigidbody>();
        body = gameObject.GetComponent<Rigidbody>();
        body.useGravity = false;
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.white;
        gameObject.transform.localScale = new Vector3(randomMass, randomMass, randomMass);

        body.mass = randomMass;
        body.position = initialPosition; // Default location
        body.velocity = initialVelocity; // The extra velocity makes the mover orbit
        findWindowLimits();

    }
    //Constrain the forces with (arbitrary) angular motion

    public Quaternion constrainAngularMotion(Vector3 angularForce)
    {
        //Calculate angular acceleration according to the acceleration's X horizontal direction and magnitude
        Vector3 aAcceleration = new Vector3(angularForce.x, 0f, 0f);
        Quaternion bodyRotation = body.rotation;
        bodyRotation.eulerAngles += new Vector3(aAcceleration.x, 0f, 0f);
        bodyRotation.x = Mathf.Clamp(bodyRotation.x, 0f, .1f);
        angle += bodyRotation.eulerAngles * Time.deltaTime;
        angleRotation = Quaternion.Euler(angle.x, angle.y, angle.z);
        return angleRotation;
    }

    //Checks to ensure the body stays within the boundaries
    public void CheckEdges()
    {
        Vector2 velocity = body.velocity;
        if (body.position.x > maximumPos.x || body.position.x < minimumPos.x)
        {
            velocity.x *= -1 * Time.deltaTime; ;
        }
        if (body.position.y > maximumPos.y || body.position.y < minimumPos.y)
        {
            velocity.y *= -1 * Time.deltaTime; ;
        }
        body.velocity = velocity;
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
//}
