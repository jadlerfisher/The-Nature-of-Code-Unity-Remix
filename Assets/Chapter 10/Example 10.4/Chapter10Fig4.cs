using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter10Fig4 : MonoBehaviour
{
    Network10_4 network;
    Vector2 maximumPos;

    // Start is called before the first frame update
    void Start()
    {
        FindWindowLimits();

        // Create the Network Object
        network = new Network10_4(maximumPos.x / 2, maximumPos.y / 2);

        // Create a set of neurons
        Neuron10_4 a = new Neuron10_4(-23, 0);
        Neuron10_4 b = new Neuron10_4(-15, 0);
        Neuron10_4 c = new Neuron10_4(0, 7);
        Neuron10_4 d = new Neuron10_4(0, -7);
        Neuron10_4 e = new Neuron10_4(15, 0);
        Neuron10_4 f = new Neuron10_4(23, 0);

        // Connect them
        network.Connect(a, b , 1f);
        network.Connect(b, c, Random.Range(0f, 1f));
        network.Connect(b, d, Random.Range(0f, 1f));
        network.Connect(c, e, Random.Range(0f, 1f));
        network.Connect(d, e, Random.Range(0f, 1f));
        network.Connect(e, f, 1f);


        // Add them to the network
        network.AddNeuron(a);
        network.AddNeuron(b);
        network.AddNeuron(c);
        network.AddNeuron(d);
        network.AddNeuron(e);
        network.AddNeuron(f);

        // Create the network visualization
        network.Display();
    }

    // Update is called once per frame
    void Update()
    {
        network.UpdateCognition();

        if (Time.frameCount % 500 == 0)
        {
            network.FeedForward(Random.Range(0f, 1f));
        }
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

public class Neuron10_4
{
    // Neuron has a position
    public Vector2 position;
    GameObject neuronGO;

    // Neuron has a list of connections
    List<Connection10_4> connections = new List<Connection10_4>();

    // We can now track the inputs and Sum them
    float sum = 0;

    // Scale the neuron size
    float r = 32;

    public Neuron10_4(float x, float y)
    {
        position = new Vector2(x, y);
        neuronGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer r = neuronGO.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        neuronGO.transform.position = position;
    }

    // Add a connection
    public void AddConnection(Connection10_4 c)
    {
        connections.Add(c);
    }

    // Receive an input
    public void FeedForward(float input)
    {
        // Accumulate it
        sum += input;
        // Activate it?
        if (sum > 1)
        {
            neuronGO.transform.localScale = new Vector3(3f, 3f, 3f);
            Fire();
            sum = 0;
        }
        else
        {
            neuronGO.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void Fire()
    {
        foreach(Connection10_4 c in connections)
        {
            c.FeedForward(sum);
        }
    }
}

public class Connection10_4
{
    // Connection from Neuron A to B
    Neuron10_4 a;
    Neuron10_4 b;

    // Connection has a weight
    float weight;

    // Variables to track the cognition
    bool sending = false;
    Vector2 sender;

    // Need to store the output for when it is time to pass it along
    float output = 0;

    GameObject theLine;
    LineRenderer lR;
    GameObject cognateGO;

    public Connection10_4(Neuron10_4 from, Neuron10_4 to, float w)
    {
        weight = w;
        a = from;
        b = to;

        // Create the cognate Game Object that will signify thinking
        cognateGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer r = cognateGO.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        r.material.color = Color.red;
        cognateGO.transform.position = from.position;
    }

    public void FeedForward(float val)
    {
        // Compute output
        output = val * weight;
        // Start animation
        sender = a.position;
        // Turn on send
        sending = true;
    }

    public void Cognition()
    {
        if (sending)
        {
            cognateGO.transform.position = sender;
            Renderer r = cognateGO.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Diffuse"));
            r.material.color = Color.red;

            // Move our position a step closer to the target.
            float step = 3.0f * Time.deltaTime;
            sender = Vector2.MoveTowards(sender, b.position, step);

            // How far we we from neuron b?
            float d = Vector2.SqrMagnitude(sender - b.position);

            // If we are flose enough, stop sending
            if (d <= 0f)
            {
                // Pass along the output!
                b.FeedForward(output);
                sending = false;
                cognateGO.transform.position = sender;
            }
        }
    }

    // Draw as a Line
    public void Display()
    {
        theLine = new GameObject();
        lR = theLine.AddComponent<LineRenderer>();
        lR.material = new Material(Shader.Find("Diffuse"));
        lR.material.color = Color.black;
        lR.SetPosition(0, new Vector2(a.position.x, a.position.y));
        lR.SetPosition(1, new Vector2(b.position.x, b.position.y));
        lR.widthMultiplier = weight;

        if (sending)
        {
            cognateGO.transform.position = sender;
        }
    }
}

public class Network10_4
{
    // The Network has a list of neurons
    List<Neuron10_4> neurons;
    // The list of connections
    List<Connection10_4> connections;

    Vector2 position;

    public Network10_4(float x, float y)
    {
        position = new Vector2(x, y);
        neurons = new List<Neuron10_4>();
        connections = new List<Connection10_4>();
    }

    // We can add a Neuron
    public void AddNeuron(Neuron10_4 n)
    {
        neurons.Add(n);
    }

    // We can connect the two Neurons
    public void Connect(Neuron10_4 a, Neuron10_4 b, float w)
    {
        Connection10_4 c = new Connection10_4(a, b, w);
        a.AddConnection(c);
        //Also add the connection
        connections.Add(c);
    }

    public void FeedForward(float input)
    {
        Neuron10_4 start = neurons[0];
        start.FeedForward(input);
    }

    public void UpdateCognition()
    {
        foreach(Connection10_4 c in connections)
        {
            c.Cognition();
        }
    }

    public void Display()
    {
        foreach(Connection10_4 c in connections)
        {
            c.Display();
        }
    }
}