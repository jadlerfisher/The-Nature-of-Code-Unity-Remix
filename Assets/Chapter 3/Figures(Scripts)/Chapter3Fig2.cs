using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig2 : MonoBehaviour
{
    // Geometry defined in the inspector.
    public float ceilingY;
    public float floorY;
    public float leftWallX;
    public float rightWallX;

    // Movers 
    private List<mover3_2> Movers = new List<mover3_2>();
    public Transform moverSpawnTransform;

    Vector3 force;

    public Rigidbody attractor;

    // Start is called before the first frame update
    void Start()
    {

        // Create copys of our mover and add them to our list

        while (Movers.Count < 30)
        {
            // Instantiate them at random vectors from the left to the right wall and from our floor to ceiling.
            moverSpawnTransform.position = new Vector2(UnityEngine.Random.Range(leftWallX, rightWallX), UnityEngine.Random.Range(floorY, ceilingY));

            Movers.Add(new mover3_2(
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
            //Now that we are sure that our Mover will not attract itself, we need it to attract a different Mover
            //We do that by directing a Mover[j] to use their attract() meothd on a Movers[i] Rigidbody
            Vector2 attractedMover = Movers[i].attract(attractor);
            //Now let's constrain the angular velocity
            Quaternion constrainedRotation = Movers[i].constrainAngularMotion(attractedMover);
            Movers[i].body.MoveRotation(constrainedRotation); 
            //We then apply that force the Movers[i] with the Rigidbody's Addforce method
            Movers[i].body.AddForce(attractedMover, ForceMode.Impulse);

            Movers[i].CheckBoundaries();
            }            
        }
    }

public class mover3_2
{
    public Rigidbody body;
    private GameObject gameObject;
    private float side;

    private Vector3 angle;
    private Quaternion angleRotation;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    public mover3_2(Vector3 position, float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;


        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //We need to create a new material for WebGL
        Renderer r = gameObject.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        body = gameObject.AddComponent<Rigidbody>();
        // Remove functionality that come with the primitive that we don't want
        gameObject.GetComponent<Collider>().enabled = false;
        UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());

        // Generate random properties for this mover
        side = UnityEngine.Random.Range(0.1f, .5f);

        // Place our mover at the specified spawn position relative
        // to the bottom of the sphere
        gameObject.transform.position = position + Vector3.up * side;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        gameObject.transform.localScale = 2 * side * Vector3.one;

        // We need to calculate the mass of the cube.
        // Assuming the cube is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = side * side * side;
        //Make sure the regular gravity is on
        body.useGravity = true;
        //Turn off the angular drag as well
        body.angularDrag = 0f;
    }

    //Special attractor for 3.2
    public Vector2 attract(Rigidbody m)
    {
        Vector2 difference = body.position - m.position;
        float dist = Mathf.Clamp(difference.magnitude, 10f, 25f);
        Vector3 gravityDirection = difference.normalized;
        float gravity = -9.81f * (m.mass * body.mass) / (dist * dist);
        Vector2 gravityVector = (gravityDirection * gravity);
        return gravityVector;
    }

    //Constrain the forces with (arbitrary) angular motion

    public Quaternion constrainAngularMotion(Vector3 angularForce)
    {
        //Calculate angular acceleration according to the acceleration's X horizontal direction and magnitude
        Vector3 aAcceleration = new Vector3(angularForce.x / 10.0f, 0f, 0f);
        Quaternion bodyRotation = body.rotation;
        bodyRotation.eulerAngles += new Vector3(aAcceleration.x, aAcceleration.y, aAcceleration.z);
        bodyRotation.x = Mathf.Clamp(bodyRotation.x, -.1f, .1f);
        bodyRotation.y = Mathf.Clamp(bodyRotation.y, -.1f, .1f);
        bodyRotation.z = Mathf.Clamp(bodyRotation.z, -.1f, .1f);
        angle += bodyRotation.eulerAngles;
        angleRotation = Quaternion.Euler(angle.x, angle.y, angle.z);
        return angleRotation;
    }

    //Checks to ensure the body stays within the boundaries
    public void CheckBoundaries()
    {
        // Using the absolute value here is and important safe
        // guard for the scenario that it takes multiple ticks
        // of FixedUpdate for the mover to return to its boundaries.
        // The intuitive solution of flipping the velocity may result
        // in the mover not returning to the boundaries and flipping
        // direction on every tick. (Mathf.Abs)

        Vector3 restrainedVelocity = body.velocity;

        if (body.position.y - side < yMin)
        {
            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
        }
        else if (body.position.y + side > yMax)
        {
            restrainedVelocity.y = -Mathf.Abs(restrainedVelocity.y);

        }

        if (body.position.x - side < xMin)
        {
            restrainedVelocity.x = Mathf.Abs(restrainedVelocity.x);


        }
        else if (body.position.x + side > xMax)
        {
            restrainedVelocity.x = -Mathf.Abs(restrainedVelocity.x);

        }

        body.velocity = restrainedVelocity;

    }

}
