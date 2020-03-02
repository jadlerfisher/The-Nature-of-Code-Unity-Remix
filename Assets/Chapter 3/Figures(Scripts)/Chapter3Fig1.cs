using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig1 : MonoBehaviour
{

    public GameObject Baton;
   
    // We are going to rotate on the z-axis. We'll rename this as a velocity;
    public Vector3 aVelocity = new Vector3(0f, 0f, 0f);
    public Vector3 aAcceleration = new Vector3(0f, 0f, .1f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aVelocity += aAcceleration;
        Baton.transform.Rotate(aVelocity, Space.World);

    }
}
