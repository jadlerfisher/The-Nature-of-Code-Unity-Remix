using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example8_6 : MonoBehaviour
{
    // Get the material for the branch(Neccasary for WebGL).
    public Material branchMaterial;

    // Parameters for generating the tree fractal.
    public float width = 0.05f;
    public Vector2 startPoint = Vector2.zero;
    public float startLength = 5;
    public float startAngle = 90;
    [Range(0, 0.9f)]
    public float childScale = 0.75f;
    public float childAngle = 30;
    [Range(0, 2)]
    public float minimumLength = 0.2f;

    void Start()
    {
        // Convert the parameters to radians.
        childAngle *= Mathf.Deg2Rad;
        startAngle *= Mathf.Deg2Rad;
        // Start the recursive branch function.
        Branch(startPoint, startAngle, startLength);
    }

    void Branch(Vector2 start, float angle, float length)
    {
        // Find the end position of this branch.
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 end = start + direction * length;

        // Create the line renderer for this branch.
        LineRenderer renderer = new GameObject().AddComponent<LineRenderer>();
        renderer.material = branchMaterial;
        renderer.widthMultiplier = width;
        renderer.positionCount = 2;
        renderer.SetPosition(0, start);
        renderer.SetPosition(1, end);

        // Create left and right branches depending on this branch length.
        if(length > minimumLength)
        {
            Branch(end, angle - childAngle, length * childScale);
            Branch(end, angle + childAngle, length * childScale);
        }
    }
}
