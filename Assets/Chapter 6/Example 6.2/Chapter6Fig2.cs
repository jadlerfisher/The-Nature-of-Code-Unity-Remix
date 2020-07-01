using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig2 : MonoBehaviour
{
    public Camera camera;
    public GameObject vehicle;
    public GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        vehicle = Instantiate(vehicle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //set gameObject's position equal to the mouse's;
        target.transform.position = MousePosition(camera);
        vehicle.GetComponent<vehicleChapter6_2>().arrive(target.transform.position);
    }

    Vector2 MousePosition(Camera camera)
    {
        // Track the Vector2 of the mouse's position
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
