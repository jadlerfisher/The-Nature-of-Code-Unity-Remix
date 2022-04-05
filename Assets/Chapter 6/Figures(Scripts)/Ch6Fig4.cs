using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch6Fig4 : MonoBehaviour
{
    public GameObject vehicle;
    public flowFieldChapter6_4 flowField;
    private vehicleChapter6_4f vC64f;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = Instantiate(vehicle, new Vector3(60, 30,0), Quaternion.identity);
        vC64f = vehicle.GetComponent<vehicleChapter6_4f>();
    }

    // Update is called once per frame
    void Update()
    {
        vC64f.follow(flowField);
    }
}
