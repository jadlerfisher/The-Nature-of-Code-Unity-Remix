using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repellerChapter4Fig7 : MonoBehaviour
{
    float strength = 100f;
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
        Vector3 dir = location - mover.location;

        float d = dir.magnitude;
        Vector3 gravityDirection = dir.normalized;
        gravityDirection.x = Mathf.Clamp(gravityDirection.x, 5, 100);
        gravityDirection.y = Mathf.Clamp(gravityDirection.y, 5, 100);
        gravityDirection.z = Mathf.Clamp(gravityDirection.z, 5, 100);
        float force = -1 * strength / (d * d);
        gravityDirection *= force;
        return gravityDirection;
    }

}
