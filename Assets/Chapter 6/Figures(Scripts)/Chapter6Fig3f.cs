using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig3f : MonoBehaviour
{
    public GameObject vehicle;
    public GameObject[] walls;
    private List<Vector3> wallPositions = new List<Vector3>();
    private vehicleChapter6_3f vC63f;
    float dist;
    Vector3 center = new Vector3(0.9f, -2.6f, -5.6f);
    // Start is called before the first frame update
    void Start()
    {
        vehicle = Instantiate(vehicle);
        vC63f = vehicle.GetComponent<vehicleChapter6_3f>();

        walls = GameObject.FindGameObjectsWithTag("wall");

        foreach (GameObject wall in walls)
        {
            wallPositions.Add(wall.transform.position);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Vector3 wall in wallPositions)
        {
            dist = Vector3.Distance(wall, vC63f.location);
            Debug.Log(dist + "this is dist");
            if (dist < 30f)
            {
                vC63f.wander();
                Debug.Log("wander wander");
            }
            else
            {

                vC63f.seek(center);
            }
        }

    }
}
