using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig6 : MonoBehaviour
{
    public GameObject vehicle;
    public pathChapter66 path;
    private vehicleChapter6_6f vC66f;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = Instantiate(vehicle, new Vector3(3.378166f, 49.9f, 0), Quaternion.identity);
        vC66f = vehicle.GetComponent<vehicleChapter6_6f>();
    }

    // Update is called once per frame
    void Update()
    {
        if (path.pathCreated)
        {
            vC66f.pathFollow(path);

        }
        else
        {
            Debug.Log("no path to walk, young son");
        }
    }


}