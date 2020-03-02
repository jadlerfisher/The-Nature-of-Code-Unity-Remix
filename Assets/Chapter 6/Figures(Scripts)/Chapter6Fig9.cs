using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig9 : MonoBehaviour
{

    public List<GameObject> vehicles = new List<GameObject>();

    public GameObject vehicle;
    private vehicleChapter6_9 vC69;
   // public GameObject target;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i <100; i++)
        {
            vehicle = Instantiate(vehicle, new Vector3(Random.Range(-10,10),Random.Range(-10,10), Random.Range(-10, 10)), Quaternion.identity);
            vehicles.Add(vehicle);
        }


    }

    // Update is called once per frame
    void Update()
    {

        foreach(GameObject vehicle in vehicles)
        {
            vehicle.GetComponent<vehicleChapter6_9>().flock(vehicles);

        }

    }


}