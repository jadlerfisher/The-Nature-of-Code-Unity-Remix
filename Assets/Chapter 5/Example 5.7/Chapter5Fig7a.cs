using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter5Fig7a : MonoBehaviour
{
    caterpillarJoint caterpillar;

    [SerializeField]
    int tailSegementCount;


    // Start is called before the first frame update
    void Start()
    {
        caterpillar = new caterpillarJoint(Vector3.zero, tailSegementCount);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        caterpillar.step();
    }
}

public class caterpillarJoint
{
    GameObject head;
    List<Rigidbody> tailRbs = new List<Rigidbody>();
    List<LineRenderer> tailLines = new List<LineRenderer>();
    public Rigidbody headRb;

    //Perlin
    float heightScale;
    float widthScale;

    public caterpillarJoint(Vector3 position, int tailSegments)
    {
        head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.transform.localPosition = position;

        head.transform.localScale = new Vector3(2f, 2f, 2f);

        //We need to create a new material for WebGL
        Renderer r = head.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        r.material.color = Color.blue;

        headRb = head.AddComponent<Rigidbody>();
        headRb.mass = 100f;

        for (int i = 0; i < tailSegments; i++)
        {

            GameObject tail = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tail.transform.localScale = new Vector3(1f, 1f, 1f);
            tail.transform.localPosition = new Vector3(position.x - head.transform.localScale.x, 0f, position.z);

            //We need to create a new material for WebGL
            Renderer t = tail.GetComponent<Renderer>();
            t.material = new Material(Shader.Find("Diffuse"));
            t.material.color = Color.red;

            Rigidbody tailRb = tail.AddComponent<Rigidbody>();
            SpringJoint tailSpringJoint = tail.AddComponent<SpringJoint>();
            tailRbs.Add(tailRb);

            tailSpringJoint.autoConfigureConnectedAnchor = false;

            tailSpringJoint.anchor = new Vector3(.5f, 0f, 0f);
            tailSpringJoint.connectedAnchor = new Vector3(-.5f, 0f, 0f);

            tailSpringJoint.minDistance = .001f;
            tailSpringJoint.maxDistance = .001f;

            //Connect to the head if it is the last tail segment, otherwise add to the previous rb
            if (i == 0)
            {
                tailSpringJoint.connectedBody = headRb;


            }
            else if (i < tailSegments)
            {
                tailSpringJoint.connectedBody = tailRbs[i - 1];
            }
        }

    }

    public void step()
    {
        widthScale += .01f;
        heightScale += .01f;

        float height = heightScale * Mathf.PerlinNoise(Time.time * .5f, 0.0f);
        float width = widthScale * Mathf.PerlinNoise(Time.time * 1, 0.0f);
        Vector3 pos = head.transform.position;
        pos.z = height;
        pos.x = width;
        head.transform.position = pos;
    }




}