using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attractorChapter2_6 : MonoBehaviour
{

    float G = 657f;
    float mass;
    Vector3 location;

    void Start() {

        mass = 10000f;
        location = this.gameObject.transform.position;
    }

    void FixedUpdate() { 
    
    
    
    }


    public Vector3 attract(moverChapter2_6 mover) {
       Vector3 difference = location - mover.location;
        float dist = difference.magnitude;
        Vector3 gravityDirection = difference.normalized;
        float gravity = 6.7f * (mass * mover.mass) / (dist * dist);

        Vector3 gravityVector = (gravityDirection * gravity);

        return gravityVector;
    }


}
