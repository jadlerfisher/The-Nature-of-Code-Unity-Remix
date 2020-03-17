using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pendulum : MonoBehaviour
{

    Vector3 location;
    Vector3 origin = new Vector3(0f,10f,0f);

    public float r;
    public float angle;
    public float aVelocity =0.0f;
    public float aAcceleration = 0.0f;
    public float damping = 0.995f;
    GameObject lineDrawing;
    LineRenderer lineRender;

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
        
    }

    public void Go() {

        float gravity = .4f;
        aAcceleration = -1 * gravity * Mathf.Sin(angle);
        aVelocity += aAcceleration;
        angle += aVelocity;

        this.gameObject.transform.position = new Vector3(r * Mathf.Sin(angle), r * Mathf.Cos(angle), 0f);
        this.gameObject.transform.position += origin;


        lineRender.SetPosition(0, origin);
        lineRender.SetPosition(1, this.gameObject.transform.position);
    }
}
