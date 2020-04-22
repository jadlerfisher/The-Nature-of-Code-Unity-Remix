using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig3 : MonoBehaviour
{
    private List<Mover2_3> Movers = new List<Mover2_3>();
    // Define constant forces in our environment
    private Vector3 wind = new Vector3(0.004f, 0f, 0f);
    private Vector3 gravity = Vector3.down * 9;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 moverSpawnPosition = new Vector3(-7, 3, 0);
        // Create copys of our mover and add them to our list
        while(Movers.Count < 30)
        {
            Movers.Add(new Mover2_3(moverSpawnPosition));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Apply the forces to each of the Movers
        foreach(Mover2_3 mover in Movers)
        {
            // ForceMode.Acceleration ignores mass
            mover.body.AddForce(gravity, ForceMode.Acceleration);
            // ForceMode.Impulse takes mass into account
            mover.body.AddForce(wind, ForceMode.Impulse);

            mover.CheckBoundaries();
        }
    }
}

public class Mover2_3
{
    public Rigidbody body;
    private GameObject gameObject;
    private float radius;

    public Mover2_3(Vector3 position)
    {
        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        body = gameObject.AddComponent<Rigidbody>();
        // Remove functionality that come with the primitive that we don't want
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());
        body.useGravity = false;

        // Generate random properties for this mover
        radius = Random.Range(0.2f, 0.6f);

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
    }

    // Checks to ensure the body stays within the boundaries
    public void CheckBoundaries()
    {
        Vector3 restrainedVelocity = body.velocity;
        if (body.position.y - radius < -4.5f)
        {
            restrainedVelocity.y = Mathf.Abs(restrainedVelocity.y);
        }
        if (body.position.x - radius < -8f)
        {
            restrainedVelocity.x = Mathf.Abs(restrainedVelocity.x);
        }
        else if(body.position.x + radius > 8f)
        {
            restrainedVelocity.x = -Mathf.Abs(restrainedVelocity.x);
        }
        body.velocity = restrainedVelocity;
    }
}
