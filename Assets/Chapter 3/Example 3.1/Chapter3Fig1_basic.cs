using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig1_basic : MonoBehaviour
{

    public GameObject Baton;
    float xAngle;
    float yAngle;
    float zAngle = 3.3f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Baton.transform.Rotate(xAngle, yAngle, zAngle, Space.World);

    }
}
