using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloop : MonoBehaviour
{
    Rigidbody rb; // The bloop's Rigid Body

    DNAbloop dna; // A bloop now has DNA

    float maxSpeed; // Keep the max speed for a Bloop
    float size; // The size of the bloop

    float health = 100; // A bloop is born with 100 hp

    float xoff, yoff; // Some variables for Perlin noise calculations

    private Vector2 maximumPos;
    Material color;

    void Start()
    {
        FindWindowLimits();
        SetDNA(new DNAbloop());

        xoff = Random.Range(-1f, 1f); // Create a seed for the random motion
        yoff = Random.Range(-1f, 1f);

        transform.position = new Vector2(Random.Range(-maximumPos.x, maximumPos.x), Random.Range(-maximumPos.y, maximumPos.y)); // Set a random location for the bloop

        rb = gameObject.AddComponent<Rigidbody>(); // Add a RigidBody for physics
        rb.useGravity = false; // and turn off gravity since we wont need it
        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        MeshRenderer r = gameObject.gameObject.GetComponent<MeshRenderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        r.material.color = Color.red;
        color = gameObject.GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        float vx = ExtensionMethods.Map(Mathf.PerlinNoise(xoff, 0), 0, 1, -maxSpeed, maxSpeed);
        float vy = ExtensionMethods.Map(Mathf.PerlinNoise(0, yoff), 0, 1, -maxSpeed, maxSpeed);
        Vector2 velocity = new Vector2(vx, vy); // a little Perlin noise algorithm to calculate a velocity;
        xoff += 0.01f;
        yoff += 0.01f;
        rb.velocity = velocity; // Set the bloop's velocity

        health -= 10 * Time.deltaTime; // Death is always looming! (We multiply by Time.delta time to not rely on framerate which may change!)

        if (Dead()) 
        {
            Destroy(gameObject);
        }

        BloopColor();
        CheckEdges();
    }

    void BloopColor()
    {
        // This blob of code makes the spheres fade from red, to blue, to green, to white as it gains more health
        float healthRed;
        if (health < 500)
            healthRed = ExtensionMethods.Map(health, 0, 500, 0, 255);
        else if (health < 2000)
            healthRed = ExtensionMethods.Map(health, 500, 1000, 255, 0);
        else
            healthRed = ExtensionMethods.Map(health, 2000, 2500, 0, 255);
        if (healthRed > 255)
            healthRed = 255;
        else if (healthRed < 0)
            healthRed = 0;

        float healthBlue;
        if (health < 1000)
            healthBlue = ExtensionMethods.Map(health, 500, 1000, 0, 255);
        else if (health < 2000)
            healthBlue = ExtensionMethods.Map(health, 1000, 1500, 255, 0);
        else
            healthBlue = ExtensionMethods.Map(health, 2000, 2500, 0, 255);
        if (healthBlue > 255)
            healthBlue = 255;
        else if (healthBlue < 0)
            healthBlue = 0;

        float healthGreen = ExtensionMethods.Map(health, 1000, 1500, 0, 255);
        if (healthGreen > 255)
            healthGreen = 255;
        else if (healthGreen < 0)
            healthGreen = 0;
        color.SetColor("_Color", new Color(healthRed, healthGreen, healthBlue));
    }

    void CheckEdges()
    {
        // Enable screen wrapping
        Vector2 pos = transform.position;
        if (pos.x < -maximumPos.x)
        {
            pos.x = maximumPos.x;
        }
        if (pos.x > maximumPos.x)
        {
            pos.x = -maximumPos.x;
        }
        if (pos.y < -maximumPos.y)
        {
            pos.y = maximumPos.y;
        }
        if (pos.y > maximumPos.y)
        {
            pos.y = -maximumPos.y;
        }
        transform.position = pos;
    }

    // This will be called everytime the bloop touches anything
    private void OnTriggerEnter(Collider other)
    {
        // First we need to check if the other thing is food!
        // From before, we set food to layer 9 so let's check if the other happened on that layer
        if (other.gameObject.layer == 9) 
        {
            // Now we know we have a piece of food!
            // Let's pick it up and give ourself some health
            health += 100;
            Destroy(other.gameObject); // The food is no longer avaliable for other bloops!
            Reproduce();
        }
    }

    GameObject Reproduce() // This function will return a new bloop, the child.
    {
        if (Random.Range(0f, 1f) < 0.20) // a 20% chance of executine the code. i.e. a 20% chance of reproducing
        {
            DNAbloop childDNA = dna.Copy(); // make a copy of the DNA
            childDNA.Mmutate(0.001f); // 0.1% mutation rate

            GameObject bloopObj = GameObject.CreatePrimitive(PrimitiveType.Sphere); // Make an object
            Bloop bloop = bloopObj.AddComponent<Bloop>(); // Turn it into a bloop!
            bloop.SetDNA(childDNA);

            bloopObj.transform.position = transform.position; // Set to the same location as parent

            return bloopObj;
        }
        else 
        {
            return null;
        }
    }
    public bool Dead()
    { // We can use this to tell us if the bloop is dead or alive
        if (health < 0)
            return true;
        else
            return false;
    }

    public void SetDNA(DNAbloop newDNA)
    {
        dna = newDNA;
        maxSpeed = ExtensionMethods.Map(dna.genes[0], 0, 1, 10, 0); // MaxSpeed an Size are now mapped to values according to the DNA
        size = ExtensionMethods.Map(dna.genes[0], 0, 1, 0, 2);

        gameObject.transform.localScale *= size;
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // Set the desired camera size
        Camera.main.orthographicSize = 5;

        // Set position of the camera to ensure 0,0 x,y
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}


public class DNAbloop 
{
    public float[] genes;
    public DNAbloop() 
    {
        /*
         * It may seem absurd to use an array when all we have 
         * is a single value, but we stick with an array in case we 
         * want to make more sophisticated bloops later. 
         */
        genes = new float[1];
        for (int i = 0; i < genes.Length; i++) 
        {
            genes[i] = Random.Range(0f, 1f);
        }
    }

    public DNAbloop(float[] genes_) 
    {
        genes = genes_;
    }

    public void Mmutate(float rate) 
    {
        for(int i = 0; i < genes.Length; i++) 
        {
            genes[i] += Random.Range(-rate, rate); // We'll change each gene by + or - the rate at random
        }
    }

    public DNAbloop Copy() 
    {
        float[] newgenes = new float[genes.Length];
        newgenes = (float[])genes.Clone();
        return new DNAbloop(newgenes);
    }
}