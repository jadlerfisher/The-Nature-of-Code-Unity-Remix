using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example8_6 : MonoBehaviour
{
    // Get the material for the branch(Neccasary for WebGL).
    [SerializeField] Material branchMaterial;

    // Parameters for generating the tree fractal.
    [SerializeField] float width = 0.05f;
    [SerializeField] float startLength = 5;
    [Range(0, 0.9f)]
    [SerializeField] float childScale = 0.75f;
    [SerializeField] float childAngle = 30;
    [Range(0, 2)]
    [SerializeField] float minimumLength = 0.2f;

    void Start()
    {
        // Start the recursive branch function.
        Branch(transform, startLength);
    }

    void Branch(Transform parentRoot, float length)
    {
        // Determine where this branch ends.
        Vector2 end = parentRoot.position + parentRoot.up * length;

        // Create the line renderer for this branch.
        DrawLine(parentRoot.gameObject, parentRoot.position, end);

        // Create left and right branches depending on this branch length.
        if(length > minimumLength)
        {
            // Create a new child transform for this branch.
            Transform leftRoot = new GameObject().transform;
            leftRoot.parent = parentRoot;

            // Set the position and rotation relative to the previous branch.
            leftRoot.localPosition = Vector2.up * length;
            leftRoot.localEulerAngles = new Vector3(0, 0, childAngle);

            // Call this function again.
            Branch(leftRoot, length * childScale);

            // Repeat for the right branch, but with the opposite angle.
            Transform rightRoot = new GameObject().transform;
            rightRoot.parent = parentRoot;
            rightRoot.localPosition = Vector2.up * length;
            rightRoot.localEulerAngles = new Vector3(0, 0, -childAngle);
            Branch(rightRoot, length * childScale);
        }
    }

    void DrawLine(GameObject parent, Vector2 start, Vector2 end)
    {
        LineRenderer renderer = parent.GetComponent<LineRenderer>();
        if (renderer == null)
        {
            renderer = parent.AddComponent<LineRenderer>();
        }
        renderer.positionCount = 2;
        renderer.SetPosition(0, start);
        renderer.SetPosition(1, end);
        renderer.material = branchMaterial;
        renderer.widthMultiplier = width;
    }
}
