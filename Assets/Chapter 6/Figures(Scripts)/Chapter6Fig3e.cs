using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig3e : MonoBehaviour
{
    public GameObject vehicle;
    vehicleChapter6_3e vC63e;
    void Start()
    {
        vehicle = Instantiate(vehicle);
        vC63e = vehicle.GetComponent<vehicleChapter6_3e>();
    }

    // Update is called once per frame
    void FixedUpdate()
        {

            vC63e.wander();
        }    
}
