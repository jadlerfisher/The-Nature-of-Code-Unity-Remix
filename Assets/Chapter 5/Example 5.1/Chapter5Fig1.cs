using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter5Fig1 : MonoBehaviour
{
    //A Mover that doesn't move on its own but is
    //influenced by Physics.gravity
    gravityMover m;
    
    [SerializeField]
    int moverCount;

    //Create a List to hold the gravity movers
    List<gravityMover> gravityMovers = new List<gravityMover>();

    // Start is called before the first frame update
    void Start()
    {
        //Begin by setting the world gravity to 0
        Physics.gravity = new Vector2(0, -1f);

        //Instantiate a number of gravityMovers
        for (int i = 0; i < moverCount; i++)
        {
            m = new gravityMover();
            gravityMovers.Add(m);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Cycle through the list and have each gravityMover
        //check its position and change gravity if necessary
        foreach (gravityMover m in gravityMovers)
        {
            m.CheckEdgesChangeGravity();
            m.calculateDrag();
        }
    }
}

public class gravityMover
{
    // The basic properties of a mover class
    public Transform transform;
    public Rigidbody body;

    private Vector2 minimumPos, maximumPos;

    private GameObject mover;

    public gravityMover()
    {
        mover = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        transform = mover.transform;
        mover.AddComponent<Rigidbody>();
        body = mover.GetComponent<Rigidbody>();
        body.useGravity = true;
        body.angularDrag = 0f;
        Renderer renderer = mover.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.white;

        // Generate random properties for this mover
        float radius = Random.Range(0.1f, 0.4f);

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        mover.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;

        //We are also going to constrain the movement of the gravityMovers
        //so they only go up and down
        body.constraints = RigidbodyConstraints.FreezePositionX;

        //Place the objects in a line but at different positions
        transform.position = new Vector2(Random.Range(-5, 5), 2);

        findWindowLimits();
    }


    public void calculateDrag()
    {
        //We'll calcularw a coefficient with Reynold's number for a sphere (.47)
        //D = Cd * .5 * rho * V^2 * A
        //Where D is equal to the drag, rho is the air density, V is the velocity squared , A is a reference area, and Cd is the drag coefficient.
        //In this instance, since we are making the mass equal to volume we can use body.mass. For other shapes, you may not be able do this.
        body.drag = .47f * .5f * 1.225f * ((body.velocity.y * body.velocity.y)/2) * body.mass;
    }

    //If any of the objects passes either the max or negative position, change gravity
    public void CheckEdgesChangeGravity()
    {
        if (transform.position.y > maximumPos.y)
        {
            Physics.gravity = new Vector2(0f, Random.Range(-1f, -.1f));
        } 
        else if (transform.position.y < minimumPos.y)
        {
            Physics.gravity = new Vector2(0f, Random.Range(.1f, 1f));
        }
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 4;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));        
    }
}
