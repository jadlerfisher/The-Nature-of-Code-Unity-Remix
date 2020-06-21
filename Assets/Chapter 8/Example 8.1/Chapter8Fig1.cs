using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter8Fig1 : MonoBehaviour
{
    [SerializeField] float initialRadius;
    [SerializeField] float minimumRadius;
    [SerializeField] float strokesize;
    [SerializeField] float reductionRate;

    // Start is called before the first frame update
    void Start()
    {
        var circle = new GameObject { name = "Circle" };        
        RecursiveCircle(initialRadius);
    }

    void RecursiveCircle(float radius)
    {
        var circle = new GameObject();
        circle.DrawCircle(radius, strokesize);
        if (radius > minimumRadius)
        {
            radius *= reductionRate;
            RecursiveCircle(radius);
        }
    }
}
