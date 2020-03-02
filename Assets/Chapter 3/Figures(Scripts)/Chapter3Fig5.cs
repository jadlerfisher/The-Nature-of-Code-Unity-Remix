using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig5 : MonoBehaviour
{
    float period = 22f;
    float amplitude = 20f;

    public GameObject sphere;

    //Create variables for rendering the line between two vectors
    private GameObject lineDrawing;
    private LineRenderer lineRender;

    // Start is called before the first frame update
    void Start()
    {
        // Create a GameObject that will be the line
        lineDrawing = new GameObject();

        //Add the Unity Component "LineRenderer" to the GameObject lineDrawing. We will see a bright pink line.
        lineRender = lineDrawing.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


        float x = amplitude * Mathf.Cos((2*Mathf.PI)* Time.time/period);

        sphere.transform.position = new Vector3(x, 0f, 0f);


        //Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
        //Make sure the end of the line (1) appears at the new Vector3
        Vector3 center = new Vector3(0f, 0f, 0f);
        lineRender.SetPosition(0, center);
        lineRender.SetPosition(1, sphere.transform.position);

    }
}
