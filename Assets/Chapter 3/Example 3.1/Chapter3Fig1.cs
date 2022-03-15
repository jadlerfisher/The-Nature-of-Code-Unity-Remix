using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig1 : MonoBehaviour
{
    [SerializeField] GameObject Baton;

    // We are going to rotate on the z-axis. We'll rename this as a velocity;
    [SerializeField] Vector3 aVelocity = new Vector3(0f, 0f, 0f);
    [SerializeField] Vector3 aAcceleration = new Vector3(0f, 0f, .001f);

    // Update is called once per frame
    void FixedUpdate()
    {
        aVelocity += aAcceleration;
        Baton.transform.Rotate(aVelocity, Space.World);
    }
}
