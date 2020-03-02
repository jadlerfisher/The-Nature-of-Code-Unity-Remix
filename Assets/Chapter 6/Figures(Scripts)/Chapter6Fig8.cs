using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig8 : MonoBehaviour
{

    public List<GameObject> vehicles = new List<GameObject>();

    public GameObject vehicle;
    private vehicleChapter6_8 vC68;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i <100; i++)
        {
            vehicle = Instantiate(vehicle, new Vector3(Random.Range(-30,30),Random.Range(-30,30), Random.Range(-30, 30)), Quaternion.identity);
            vehicles.Add(vehicle);
        }


    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float speed = 1f;

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;

        target.transform.position += tempVect;

        foreach(GameObject vehicle in vehicles)
        {
            vehicle.GetComponent<vehicleChapter6_8>().applyBehaviors(vehicles, target.transform.position);

        }

    }


}