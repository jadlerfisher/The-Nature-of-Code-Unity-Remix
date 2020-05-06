using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig8 : MonoBehaviour
{
    // Geometry defined in the inspector.
    public float ceilingY;
    public float floorY;
    public float leftWallX;
    public float rightWallX;

    // Movers 
    private List<Mover2_8> Movers = new List<Mover2_8>();
    public Transform moverSpawnTransform;

    Vector3 force;

    // Start is called before the first frame update
    void Start()
    {

        // Create copys of our mover and add them to our list

        while (Movers.Count < 30)
        {
            // Instantiate them at random vectors from the left to the right wall and from our floor to ceiling.
            moverSpawnTransform.position = new Vector2(UnityEngine.Random.Range(leftWallX, rightWallX), UnityEngine.Random.Range(floorY, ceilingY));

            Movers.Add(new Mover2_8(
                        moverSpawnTransform.position,
                        leftWallX,
                        rightWallX,
                        ceilingY,
                        floorY
                    ));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // We want to create two groups of movers to iterate through.
        // We do this so that an Mover[i] never tries to attract itself
        // This is seen on line 51.
        for (int i = 0; i < Movers.Count; i++)
        {
            for (int j = 0; j < Movers.Count; j++)
            {
                if (i != j)
                {
                    //Now that we are sure that our Mover will not attract itself, we need it to attract a different Mover
                    //We do that by directing a Mover[j] to use their attract() meothd on a Movers[i] Rigidbody
                    Vector2 attractedMover = Movers[j].attract(Movers[i].body);
                    //We then apply that force the Movers[i] with the Rigidbody's Addforce method
                    Movers[i].body.AddForce(attractedMover, ForceMode.Impulse);
                }

            }
            //Now we check the boundaries of our scene to make sure the movers don't fly off
            //When we use gravity, the Movers will naturally fall out of the camera's view
            // This stops that.

            Movers[i].CheckBoundaries();
        }
    }
}


public class Mover2_8
{
    public Rigidbody body;
    private GameObject gameObject;
    private float radius;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    public Mover2_8(Vector3 position, float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;


        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //We need to create a new material for WebGL
        Renderer r = gameObject.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        body = gameObject.AddComponent<Rigidbody>();
        // Remove functionality that come with the primitive that we don't want
        gameObject.GetComponent<SphereCollider>().enabled = false;
        UnityEngine.Object.Destroy(gameObject.GetComponent<SphereCollider>());

        // Generate random properties for this mover
        radius = UnityEngine.Random.Range(0.1f, .4f);

        // Place our mover at the specified spawn position relative
        // to the bottom of the sphere
        gameObject.transform.position = position + Vector3.up * radius;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        gameObject.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;
        //Make sure the regular gravity is on
        body.useGravity = true;
        //Turn off the angular drag as well
        body.angularDrag = 0f;
    }

    public Vector2 attract(Rigidbody m)
    {
        Vector2 force = body.position - m.position;
        float distance = force.magnitude;

        // Remember we need to constrain the distance so that our circle doesn't spin out of control
        distance = Mathf.Clamp(distance, 5f, 25f);

        force.Normalize();
        float strength = (9.81f * body.mass * m.mass) / (distance * distance);
        force *= strength;
        return force;

    }

    //Checks to ensure the body stays within the boundaries
    public void CheckBoundaries()
    {
        Vector2 velocity = body.velocity;
        if (body.position.x > xMax || body.position.x < xMin)
        {
            velocity.x *= -1 * Time.deltaTime;
        }
        if (body.position.y > yMax || body.position.y < yMin)
        {
            velocity.y *= -1 * Time.deltaTime;
        }
        body.velocity = velocity;

    }
}

