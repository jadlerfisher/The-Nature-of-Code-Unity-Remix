using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter10Fig2 : MonoBehaviour
{

    vehicleChapter10_2 v;

    Vector3 desired;

    List<Vector3> targets = new List<Vector3>();
    List<GameObject> targetGO = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //desired = Vector3.zero;

        makeTargets();

        v = new vehicleChapter10_2(targets.Count, new Vector3(Random.Range(-Screen.width / 100, Screen.width / 100), Random.Range(-Screen.height / 100, Screen.height / 100), 0f));
    }

    // Update is called once per frame
    void FixedUpdate()
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
                targets.Add(new Vector3(Random.Range(-Screen.width / 100, Screen.width / 100), Random.Range(-Screen.height / 100, Screen.height / 100), 0f));
                GameObject tGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Renderer r = tGO.GetComponent<Renderer>();
                r.material = new Material(Shader.Find("Diffuse"));
                tGO.transform.position = targets[i];
                targetGO.Add(tGO);

            }

        } else
        {
            targets.Clear();

            foreach(GameObject go in targetGO)
            {
                GameObject.Destroy(go);
            }

            targetGO.Clear();
            makeTargets();
        }
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
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        brain = new Perceptron10_2(n, .1f);
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        position = spawnLocation;
        maxspeed = 2f;
        maxforce = 1f;

        vehicleGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        Renderer r = vehicleGO.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        r.material.color = Color.red;
        vehicleGO.transform.position = position;
        vehicleGO.transform.localScale = new Vector3(.5f, .5f, .5f);
        vehicleBody = vehicleGO.AddComponent<Rigidbody>();
        vehicleBody.mass = 100f;
        vehicleBody.useGravity = false;

    }


    public void drive()
    {

        position.x = Mathf.Clamp(vehicleBody.position.x, -maximumPos.x, maximumPos.x);
        position.y = Mathf.Clamp(vehicleBody.position.y, -maximumPos.y, maximumPos.y);
        position.z = Mathf.Clamp(vehicleBody.position.z, 0f, 0f);

        vehicleBody.angularVelocity = Vector3.zero;
        //vehicleBody.velocity = velocity;
        vehicleBody.position = position;
    }


    void applyForce(Vector3 force)
    {
        vehicleBody.AddForce(force);
    }

    Vector3 seek(Vector3 target)
    {
        desired = target - vehicleBody.position;
        desired.Normalize();
        desired *= maxspeed;
        Vector3 steer = desired - vehicleBody.velocity;
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

        forces.Clear();
    }





}