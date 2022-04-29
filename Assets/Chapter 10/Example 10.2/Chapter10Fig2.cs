using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter10Fig2 : MonoBehaviour
{
    [SerializeField] Material targetMat;

    vehicleChapter10_2 v;

    List<Vector3> targets = new List<Vector3>();
    List<GameObject> targetGO = new List<GameObject>();

    Vector3 maximumPos;

    GameObject centerCube;

    // Start is called before the first frame update
    void Start()
    {
        // make color and alpha assignment into an extension method
        centerCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        centerCube.transform.localScale = Vector3.one * 2;
        centerCube.transform.position = Vector3.zero;
        Renderer r = centerCube.GetComponent<Renderer>();
        r.material = targetMat;

        FindWindowLimits();

        makeTargets();

        v = new vehicleChapter10_2(targets.Count, RandomWithinBounds());
    }

    void Update()
    {
        v.steer(targets);
        v.drive();

        if (Input.GetMouseButtonDown(0))
        {
            makeTargets();
        }
    }


    void makeTargets()
    {
        if (targets.Count <= 0)
        {
            for (int i = 0; i < 8; i++)
            {
                targets.Add(RandomWithinBounds());
                GameObject tGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Renderer r = tGO.GetComponent<Renderer>();
                r.material = targetMat;
                tGO.transform.localScale = Vector3.one * 2;
                tGO.transform.position = targets[i];
                targetGO.Add(tGO);
            }

        } else
        {
            targets.Clear();

            foreach(GameObject go in targetGO)
            {
                Destroy(go);
            }

            targetGO.Clear();
            makeTargets();
        }
    }

    // Make FindWindowLimits and extension method
    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    private Vector3 RandomWithinBounds()
    {
        return new Vector3(Random.Range(-maximumPos.x, maximumPos.x), Random.Range(-maximumPos.y, maximumPos.y), 0f);
    }

}

public class Perceptron10_2
{
    //The Perceptron stores its weightsand Learning Constants
    List<float> weights = new List<float>();
    float c;

    public Perceptron10_2(int n, float c_)
    {
        c = c_;
        for (int i = 0; i < n; i++)
        {
            //Weights start off random
            weights.Add(Random.Range(0f, 1f));
        }
    }

    //Return an output based on inputs
    public Vector3 feedforward(List<Vector3> forces)
    {
        Vector3 sum = Vector3.zero;

        for (int i = 0; i < weights.Count; i++)
        {
            forces[i] *= weights[i];
            sum += forces[i];
        }
        return sum;
    }


    //Train the network against known data
    public void train(List<Vector3> forces, Vector3 error)
    {
        for (int i = 0; i < weights.Count; i++)
        {
            weights[i] += c * error.x * forces[i].x;
            weights[i] += c * error.y * forces[i].y;
            weights[i] = Mathf.Clamp(weights[i], 0f, 1f);
        }
    }
}

public class vehicleChapter10_2
{
    Perceptron10_2 brain;

    public float r;
    public float maxforce;
    public float maxspeed;

    Vector3 position;
    Vector3 velocity;
    Vector3 acceleration;
    Vector3 desired;

    GameObject vehicleGO;
    Rigidbody vehicleBody;

    Vector2 maximumPos;

    public vehicleChapter10_2(int n, Vector2 spawnLocation)
    {

        // Next we grab the minimum and maximum position for the screen
        FindWindowLimits();

        brain = new Perceptron10_2(n, .1f);
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        position = spawnLocation;
        maxspeed = 4f;
        maxforce = .001f;
        desired = Vector3.zero;

        vehicleGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        Collider col = vehicleGO.GetComponent<Collider>();
        Object.Destroy(col);

        Renderer r = vehicleGO.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Diffuse"));
        mat.color = Color.red;
        r.material = mat;

        vehicleGO.transform.position = position;
        vehicleGO.transform.localScale = new Vector3(.5f, .5f, .5f);
        vehicleBody = vehicleGO.AddComponent<Rigidbody>();
        vehicleBody.mass = 1f;
        vehicleBody.useGravity = false;
    }


    public void drive()
    {
        velocity += acceleration;
        velocity.x = Mathf.Clamp(velocity.x, -maxspeed, maxspeed);
        velocity.y = Mathf.Clamp(velocity.y, -maxspeed, maxspeed);

        position.x = Mathf.Clamp(vehicleBody.position.x, -maximumPos.x, maximumPos.x);
        position.y = Mathf.Clamp(vehicleBody.position.y, -maximumPos.y, maximumPos.y);
        position.z = 0f;
        position += velocity * Time.deltaTime;
        acceleration = Vector3.zero;
        vehicleBody.angularVelocity = Vector3.zero;
        //vehicleBody.velocity = velocity;
        vehicleBody.position = position;
    }


    void applyForce(Vector3 force)
    {
        acceleration += force;
    }

    Vector3 seek(Vector3 target)
    {
        Vector3 desired = target - vehicleBody.position;
        desired.Normalize();
        desired *= maxspeed;
        Vector3 steer = desired - velocity;
        steer.x = Mathf.Clamp(steer.x, -maxforce, maxforce);
        steer.y = Mathf.Clamp(steer.y, -maxforce, maxforce);
        steer.z = Mathf.Clamp(steer.z, -maxforce, maxforce);
        return steer;
    }

    public void steer(List<Vector3> targets)
    {
        // Make an array of forces
        List<Vector3> forces = new List<Vector3>();
        forces.AddRange(targets);

        // Steer towards all targets
        for (int i = 0; i < forces.Count; i++)
        {
            forces[i] = seek(targets[i]);
        }

        // That array of forces is the input to the brain
        Vector3 result = brain.feedforward(forces);
        applyForce(result);

        // Train the brain according to the error
        Vector3 error = desired - position;
        brain.train(forces, error);

        //forces.Clear();
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}