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

    public Vector3 repel(particleChapter4_6 mover)
    {
        Vector3 dir = location - mover.location;

        float d = dir.magnitude;
        d = Mathf.Clamp(d, 5, 100);
        Vector3 gravityDirection = dir.normalized;
        float force = -1 * strength / (d * d);
        gravityDirection *= force;
        return gravityDirection;
    }
}
