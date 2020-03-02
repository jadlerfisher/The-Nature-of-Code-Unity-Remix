using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathChapter65 : MonoBehaviour
{
    public List<Vector3> pathVectors = new List<Vector3>();
    public GameObject pathStart;
    public Vector3 startVector;
    public Vector3 endVector;
    public GameObject pathEnd;
    float distCovered;
    float fractionOfJourney;

    public float speed = 10000000F;
    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public bool pathCreated = false;
    public GameObject track;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        startVector = pathStart.transform.position;
        endVector = pathEnd.transform.position;

        journeyLength = Vector3.Distance(startVector, endVector);     
        
    }

    // Update is called once per frame
    void Update()
    {
        distCovered = (Time.time - startTime) * speed;
        fractionOfJourney = distCovered / journeyLength;

        if (distCovered < journeyLength)
        {
            pathVectors.Add(Vector3.Lerp(startVector, endVector, fractionOfJourney));
            Instantiate(track, Vector3.Lerp(startVector, endVector, fractionOfJourney), Quaternion.identity);
        }
        else {
            pathCreated = true;
        }
    }
}
