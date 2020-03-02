using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig2 : MonoBehaviour
{
    public GameObject vehicle;
    public GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        vehicle = Instantiate(vehicle);
        target.transform.position = new Vector3(2f, 1f, -10f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float speed = 10f;

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;

        target.transform.position += tempVect;

        vehicle.GetComponent<vehicleChapter6_2>().arrive(target.transform.position);

    }
}
