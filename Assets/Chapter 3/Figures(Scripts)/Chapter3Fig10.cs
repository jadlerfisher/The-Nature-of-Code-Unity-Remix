using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig10 : MonoBehaviour
{


    public GameObject pendulumObject;
    private pendulum pendulum;


    float r;
    float angle;
    float aVelocity;
    float aAcceleration;
    float damping;

    // Start is called before the first frame update
    void Start()
    {
        pendulum = pendulumObject.GetComponent<pendulum>();
        pendulum.r = 125f;
    }

    // Update is called once per frame
    void Update()
    {

        float gravity = 0.4f;
        pendulum.aAcceleration = (-1*gravity/pendulum.r) * Mathf.Sin(angle);

        pendulum.aVelocity += pendulum.aAcceleration;
        pendulum.angle += pendulum.aVelocity;

        pendulum.aVelocity *= pendulum.damping;

        pendulum.Go();

    }
}
