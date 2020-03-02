using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig5 : MonoBehaviour
{
    public GameObject vehicle;
    public pathChapter65 path;
    private vehicleChapter6_5f vC65f;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = Instantiate(vehicle, new Vector3(3.378166f, 49.9f, 0), Quaternion.identity);
        vC65f = vehicle.GetComponent<vehicleChapter6_5f>();
    }

    // Update is called once per frame
    void Update()
    {
        if (path.pathCreated)
        {
            vC65f.pathFollow(path);

        } else {
            Debug.Log("no path to walk, young son");
        }
    }
}
