using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repellerChapter4Fig7 : MonoBehaviour
{
    float G = 657f;
    float mass;
    Vector3 location;

    void Start()
    {

        mass = .02f;
        location = this.gameObject.transform.position;
    }

    void FixedUpdate()
    {



    }


    public Vector3 repel(particleChapter4_6 mover)
    {
        Vector3 difference = location - mover.location;
        Debug.Log("difference" + difference);

        float dist = difference.magnitude;
        Vector3 gravityDirection = difference.normalized;
        float gravity = 6.7f * (mass * mover.mass) / (dist * dist);

        Vector3 gravityVector = (gravityDirection * gravity);
        return gravityVector;
    }

}
