using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example8_7 : MonoBehaviour
{
    // Get the material for the branch(Neccasary for WebGL).
    public Material branchMaterial;

    // Parameters for generating the tree fractal.
    public float width = 0.05f;
    public float startLength = 4;
    [Range(0, 0.9f)]
    public float childScale = 0.75f;
    public float minChildAngle = -55;
    public float maxChildAngle = 55;
    public int minBranches = 1;
    public int maxBranches = 4;
    [Range(0, 2)]
    public float minimumLength = 0.45f;

    void Start()
    {
        // Start the recursive branch function.
        Branch(transform, startLength);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Remove previous generated tree.
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            // Generate a new tree.
            Branch(transform, startLength);
        }
    }

    void Branch(Transform parentRoot, float length)
    {
        // Determine where this branch ends.
        Vector2 end = parentRoot.position + parentRoot.up * length;

        // Create the line renderer for this branch.
        DrawLine(parentRoot.gameObject, parentRoot.position, end);

        // Create left and right branches depending on this branch length.
        if (length > minimumLength)
        {
            int childBranchCount = Random.Range(minBranches, maxBranches + 1);

            for(int i = 0; i < childBranchCount; i++)
            {
                // Create a new child transform for this branch.
                Transform newBranch = new GameObject().transform;
                newBranch.parent = parentRoot;
                // Set the position and rotation relative to the previous branch.
                newBranch.localPosition = Vector2.up * length;
                newBranch.localEulerAngles = new Vector3(0, 0, Random.Range(minChildAngle, maxChildAngle));
                // Call this function again.
                Branch(newBranch, length * childScale);
            }
        }
    }

    void DrawLine(GameObject parent, Vector2 start, Vector2 end)
    {
        LineRenderer renderer = parent.GetComponent<LineRenderer>();
        if(renderer == null)
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
