﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter10Fig3 : MonoBehaviour
{
    Vector2 maximumPos;
    Network network;

    // Start is called before the first frame update
    void Start()
    {
        FindWindowLimits();

        // Create the Network Object
        network = new Network(maximumPos.x / 2, maximumPos.y / 2);

        // Create a set of neurons
        Neuron a = new Neuron(-20, 0);
        Neuron b = new Neuron(0, 7);
        Neuron c = new Neuron(0, -7);
        Neuron d = new Neuron(20, 0);

        // Connect the neurons
        network.Connect(a, b);
        network.Connect(a, c);
        network.Connect(b, d);
        network.Connect(c, d);

        // Add them to the network
        network.AddNeuron(a);
        network.AddNeuron(b);
        network.AddNeuron(c);
        network.AddNeuron(d);

        DrawNetwork();
    }

    void DrawNetwork()
    {
        network.Display();
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

public class Neuron
{
    // Neuron has a position
    public Vector2 position;

    // Neuron has a list of connections
    List<Connection> connections = new List<Connection>();

    public Neuron(float x, float y)
    {
        position = new Vector2(x, y);
    }

    // Add a connection
    public void AddConnection(Connection c)
    {
        connections.Add(c);
    }

    // Draw the Neuron as a circle
    public void Display()
    {
        GameObject neuronGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer r = neuronGO.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        neuronGO.transform.position = position;

        // Draw all of its connections
        foreach (Connection c in connections)
        {
            c.Display();
        }
    }
}

public class Connection
{
    // Connection from Neuron A to B
    Neuron a;
    Neuron b;

    // Connection has a weight
    float weight;

    GameObject theLine;
    LineRenderer lR;

    public Connection (Neuron from, Neuron to, float w)
    {
        weight = w;
        a = from;
        b = to;
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
    }
}

public class Network
{
    // The Network has a list of neurons
    List<Neuron> neurons;
    Vector2 position;

    public Network(float x, float y)
    {
        position = new Vector2(x, y);
        neurons = new List<Neuron>();
    }

    // We can add a Neuron
    public void AddNeuron(Neuron n)
    {
        neurons.Add(n);
    }

    // We can connect the two Neurons
    public void Connect(Neuron a, Neuron b)
    {
        Connection c = new Connection(a, b, Random.Range(0f, 1f));
        a.AddConnection(c);
    }

    public void Display()
    {
        foreach(Neuron n in neurons)
        {
            n.Display();
        }
    }
}