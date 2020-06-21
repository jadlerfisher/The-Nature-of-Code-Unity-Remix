using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter8Fig3 : MonoBehaviour
{
    [SerializeField] float initialRadius;
    [SerializeField] float minimumRadius;
    [SerializeField] float strokesize;

    // Start is called before the first frame update
    void Start()
    {
        var circle = new GameObject { name = "Circle" };
        RecursiveCircle(initialRadius, 0, 0);
    }

    void RecursiveCircle(float radius, float xPos, float zPos)
    {
        var circle = new GameObject();
        circle.DrawCircle(radius, strokesize, xPos, zPos);

        //Very important to make sure
        if (radius > minimumRadius)
        {            
            RecursiveCircle(radius / 2, xPos + radius, zPos);
            RecursiveCircle(radius / 2, xPos - radius, zPos);
            RecursiveCircle(radius / 2, xPos, zPos + radius);
            RecursiveCircle(radius / 2, xPos, zPos - radius);

        }
    }
}
