using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig1 : MonoBehaviour
{

    public GameObject Mover;
    private moverChapter2 moverC2;

    private Vector3 wind;
    private Vector3 gravity;

    // Start is called before the first frame update
    void Start()
    {
        Mover = Instantiate(Mover);
        moverC2 = Mover.GetComponent<moverChapter2>();

        gravity = new Vector3(0f, 0.0001f, 0f);
        wind = new Vector3(0.00001f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        moverC2.applyForce(wind);
        moverC2.applyForce(gravity);

    }
}





