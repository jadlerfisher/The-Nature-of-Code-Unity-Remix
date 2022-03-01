using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig5 : MonoBehaviour
{
    // Geometry defined in the inspector
    [SerializeField] float floorY;
    [SerializeField] float leftWallX;
    [SerializeField] float rightWallX;
    [SerializeField] Transform moverSpawnTransform;

    // Serialize the components required to create the water
    [SerializeField] Transform fluidCornerA;
    [SerializeField] Transform fluidCornerB;
    [SerializeField] Material waterMaterial;
    [SerializeField] float fluidDrag;

    // Create our lists
    private List<Mover2_5> Movers = new List<Mover2_5>();
    private List<Fluid2_5> Fluids = new List<Fluid2_5>();

    // Start is called before the first frame update
    void Start()
    {
        // Create copys of our mover and add them to our list
        while (Movers.Count < 30)
        {
            Movers.Add(new Mover2_5(moverSpawnTransform.position,leftWallX,rightWallX,floorY));
        }

        // Add the fluid to our scene
        Fluids.Add(new Fluid2_5(fluidCornerA.position,fluidCornerB.position,fluidDrag,waterMaterial));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Apply the forces to each of the Movers
        foreach (Mover2_5 mover in Movers)
        {
            // Check for interaction with any of our fluids
            foreach(Fluid2_5 fluid in Fluids)
            {
                if(mover.IsInside(fluid))
                {
                    // Apply a friction force that directly opposes the current motion
                    Vector3 friction = -mover.body.velocity;
                    friction.Normalize();
                    friction *= fluid.dragCoefficient;
                    mover.body.AddForce(friction, ForceMode.Force);
                }
            }
            mover.CheckEdges();
        }
    }
}

public class Mover2_5
{
    public Rigidbody body;
    private GameObject gameObject;
    private float radius;

    private float xMin;
    private float xMax;
    private float yMin;
    private float xSpawn;

    public Mover2_5(Vector3 position, float xMin, float xMax, float yMin)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        
        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = gameObject.AddComponent<Rigidbody>();

        // Remove functionality that come with the primitive that we don't want
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());

        // Generate random properties for this mover
        radius = Random.Range(0.2f, 0.6f);

        // Generate a random x value within the bundaries
        xSpawn = Random.Range(xMin, xMax);

        // Place our mover at a randomized spawn position relative
        // to the bottom of the sphere
        gameObject.transform.position = new Vector3(xSpawn, position.y, position.z) + Vector3.up * radius;

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        gameObject.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;
    }

    // Checks to ensure the body stays within the boundaries
    public void CheckEdges()
    {
        Vector3 restrainedVelocity = body.velocity;
        if (body.position.y - radius < yMin)
        {
            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
            body.position = new Vector3(body.position.x, yMin, body.position.z) + Vector3.up * radius;
        }
        body.velocity = restrainedVelocity;
    }

    public bool IsInside(Fluid2_5 fluid)
    {
        // Check to see if the mover is inside the range on each axis.
        if (body.position.x > fluid.minBoundary.x &&
            body.position.x < fluid.maxBoundary.x &&
            body.position.y > fluid.minBoundary.y &&
            body.position.y < fluid.maxBoundary.y &&
            body.position.z > fluid.minBoundary.z &&
            body.position.z < fluid.maxBoundary.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class Fluid2_5
{
    public Vector3 minBoundary;
    public Vector3 maxBoundary;
    public float dragCoefficient;

    public Fluid2_5(Vector3 corner1, Vector3 corner2, float dragCoefficient, Material material)
    {
        // Get the minimum and maximum corners of the rectangular prism
        // This code allows the designer to place the volume corners at
        // any of the eight possible diagonals of a rectangular prism.
        minBoundary = new Vector3(
            Mathf.Min(corner1.x, corner2.x),
            Mathf.Min(corner1.y, corner2.y),
            Mathf.Min(corner1.z, corner2.z)
        );
        maxBoundary = new Vector3(
            Mathf.Max(corner1.x, corner2.x),
            Mathf.Max(corner1.y, corner2.y),
            Mathf.Max(corner1.z, corner2.z)
        );
        this.dragCoefficient = dragCoefficient;

        // Create the presence of the object in 3D space
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.GetComponent<Renderer>().material = material;

        // Remove undesired components that come with the primitive
        obj.GetComponent<BoxCollider>().enabled = false;
        Object.Destroy(obj.GetComponent<BoxCollider>());

        // Position and scale the new cube to match the boundaries.
        obj.transform.position = (corner1 + corner2) / 2;
        obj.transform.localScale = new Vector3(
            Mathf.Abs(corner2.x - corner1.x),
            Mathf.Abs(corner2.y - corner1.y),
            Mathf.Abs(corner2.z - corner1.z)
        );
    }
}