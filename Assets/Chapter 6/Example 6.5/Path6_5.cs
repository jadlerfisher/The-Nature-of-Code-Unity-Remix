using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path6_5 : MonoBehaviour
{
    [SerializeField] public Transform startVector, endVector;
    [SerializeField] public float radius;
    [SerializeField] Material pathMaterial;

    void Start()
    {
        // Create a mesh for the path.
        GameObject path = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(path.GetComponent<MeshCollider>());
        path.GetComponent<Renderer>().material = pathMaterial;

        // Align the mesh to the path.
        // Center the path to the midpoint of the track start and end:
        path.transform.position = 0.5f * (startVector.position + endVector.position);

        // Scale the path to reflect the path length and radius:
        path.transform.localScale = new Vector3(radius * 2, Vector3.Distance(startVector.position, endVector.position), 1);

        // Rotate the path to align it to the angle between the start and end:
        path.transform.eulerAngles = Vector3.forward * Vector3.Angle(Vector3.down, endVector.position - startVector.position);
    }
}
