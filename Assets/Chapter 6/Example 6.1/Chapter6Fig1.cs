using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter6Fig1 : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject vehicle;
    [SerializeField] GameObject target;

    private VehicleChapter6_1 agent;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        agent = vehicle.GetComponent<VehicleChapter6_1>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //set gameObject's position equal to the mouse's;
        target.transform.position = MousePosition(cam);
        agent.Seek(target.transform.position);
    }
    Vector2 MousePosition(Camera camera)
    {
        // Track the Vector2 of the mouse's position
        return camera.ScreenToWorldPoint(Input.mousePosition);        
    }
}
