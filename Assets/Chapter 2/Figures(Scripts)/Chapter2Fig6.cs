using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig6 : MonoBehaviour
{

    public GameObject a;
    public GameObject m;
    Vector3 force;

    private attractorChapter2_6 aC26;
    private moverChapter2_6 mC26;


    // Start is called before the first frame update
    void Start()
    {
        a = Instantiate(a);
        m = Instantiate(m, new Vector3(4f, 0f, 0f), Quaternion.identity);

        aC26 = a.GetComponent<attractorChapter2_6>();
        mC26 = m.GetComponent<moverChapter2_6>();
 

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        force = aC26.attract(mC26);
        mC26.applyForce( new Vector3 (1f, 0f, 0f));
        mC26.applyForce(force);

    }
}
