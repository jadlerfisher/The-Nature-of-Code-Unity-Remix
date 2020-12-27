using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig6RB : MonoBehaviour
{

    public GameObject a;
    public GameObject m;
    Vector3 force;

    private attractorChapter2_6 aC26;
    private moverChapter2_6 mC26;


    // Start is called before the first frame update
    void Start()
    {



 

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 difference = this.transform.position - m.transform.position;
        float dist = difference.magnitude;
        Vector3 gravityDirection = difference.normalized;
        float gravity = 6.7f * (this.transform.localScale.x * m.transform.localScale.x * 80) / (dist * dist);

        Vector3 gravityVector = (gravityDirection * gravity);
        m.transform.GetComponent<Rigidbody>().AddForce(m.transform.forward, ForceMode.Acceleration);
        m.transform.GetComponent<Rigidbody>().AddForce(gravityVector, ForceMode.Acceleration);
    }
}
