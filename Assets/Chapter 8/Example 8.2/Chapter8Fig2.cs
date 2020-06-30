using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter8Fig2 : MonoBehaviour
{
    [SerializeField] float initialRadius;
    [SerializeField] float minimumRadius;
    [SerializeField] float strokesize;

    // Start is called before the first frame update
    void Start()
    {
        var circle = new GameObject { name = "Circle" };
        RecursiveCircle(initialRadius, 50, 0);
    }

    void RecursiveCircle(float radius, float xChange, float zChange)
    {
        var circle = new GameObject();
        circle.DrawCircle(radius, strokesize, xChange, zChange);
        if (radius > minimumRadius)
        {
            RecursiveCircle(radius / 2, xChange + radius, zChange);
            RecursiveCircle(radius / 2, xChange - radius, zChange);
        }
    }
}
