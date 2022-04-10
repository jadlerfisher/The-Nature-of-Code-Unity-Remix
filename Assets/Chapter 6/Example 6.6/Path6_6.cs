using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path6_6 : MonoBehaviour
{
    [SerializeField] public Transform[] points;
    [SerializeField] public float radius;
    [SerializeField] Material pathMaterial;

    private LineRenderer pathRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Create a line renderer to draw the path.
        pathRenderer = new GameObject().AddComponent<LineRenderer>();
        pathRenderer.generateLightingData = true;
        pathRenderer.material = pathMaterial;
        pathRenderer.widthMultiplier = radius * 2;

        // Get the path positions from the transforms.
        pathRenderer.positionCount = points.Length;
        for(int i = 0; i < points.Length; i++)
        {
            pathRenderer.SetPosition(i, points[i].position);
        }
    }
}
