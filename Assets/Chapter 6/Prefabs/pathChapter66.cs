using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathChapter66 : MonoBehaviour
{
    public List<Vector3> pathVectors = new List<Vector3>();
    public List<Vector3> secondPathVectors = new List<Vector3>();

    public GameObject pathStart;
    public GameObject pathMiddle;
    public GameObject pathEnd;
    public Vector3 startVector;
    public Vector3 middleVector;
    public Vector3 endVector;
    float distCovered;
    float fractionOfJourney;


    public float speed = 1F;
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
        middleVector = pathMiddle.transform.position;
        endVector = pathEnd.transform.position;

        journeyLength = Vector3.Distance(startVector, middleVector) + Vector3.Distance(middleVector, endVector);
        //
    }

    // Update is called once per frame
    void Update()
    {
        if (!pathCreated)
        {
            distCovered = (Time.time - startTime) * speed;
            fractionOfJourney = distCovered / journeyLength;

            if (distCovered < journeyLength)
            {
                pathVectors.Add(Vector3.Lerp(startVector, middleVector, fractionOfJourney));
                secondPathVectors.Add(Vector3.Lerp(middleVector, endVector, fractionOfJourney));
                Instantiate(track, Vector3.Lerp(startVector, middleVector, fractionOfJourney), Quaternion.identity);
                Instantiate(track, Vector3.Lerp(middleVector, endVector, fractionOfJourney), Quaternion.identity);
            } else
            {
                pathVectors.AddRange(secondPathVectors);
                pathCreated = true;
            }
        }
        else
        {
            //Debug.Log("Do nothing");
        }


    }
}
