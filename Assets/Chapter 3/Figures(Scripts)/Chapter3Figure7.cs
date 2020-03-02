using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Figure7 : MonoBehaviour
{
    Vector3 angle;
    Vector3 velocity;
    Vector3 amplitude;

    float period = 22f;

    public GameObject sphere;

    //Create variables for rendering the line between two vectors
    private GameObject lineDrawing;
    private LineRenderer lineRender;

    public List<GameObject> oscilattors = new List<GameObject>();

    Vector3 center = new Vector3(0f, 0f,0f);

    // Start is called before the first frame update
    void Start()
    {

        foreach (GameObject o in oscilattors)
        {
            //Add the Unity Component "LineRenderer" to the GameObject lineDrawing. We will see a bright pink line.
            o.AddComponent<LineRenderer>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject o in oscilattors)
        {
            Vector3 p = Camera.main.ViewportToWorldPoint(new Vector3(100, 100, Camera.main.nearClipPlane));

            velocity = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
            amplitude = new Vector3(Random.Range(p.x, p.x / 2), Random.Range(p.y, p.y / 2), Random.Range(-3f, 3f));
            
            angle += velocity;

            float x = Mathf.Sin(angle.x) * amplitude.x;
            float y = Mathf.Sin(angle.y) * amplitude.y;

            o.transform.transform.Translate(new Vector3(x, y, 0f) * Time.deltaTime);

            //Begin rendering the line between the two objects. Set the first point (0) at the centerSphere Position
            //Make sure the end of the line (1) appears at the new Vector3

            o.GetComponent<LineRenderer>().SetPosition(0, center);
            o.GetComponent<LineRenderer>().SetPosition(1, o.transform.position);
        }
    }
}
