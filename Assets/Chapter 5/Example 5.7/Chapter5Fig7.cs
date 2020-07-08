using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter5Fig7 : MonoBehaviour
{
    fixedJoint fixedJoint;
    springJoint springJoint;

    // Start is called before the first frame update
    void Start()
    {
        fixedJoint = new fixedJoint(new Vector3(0, 5f, 0f));
        springJoint = new springJoint(new Vector3(0, 2f, 0f));

        springJoint.sJoint.connectedBody = fixedJoint.body;
    }

    // Update is called once per frame
    void Update()
    {
        //Play with the fixedjoint rigidbody every frame to create some
        //force movements that act upon the spring

        fixedJoint.body.position = new Vector3(Random.Range(-3f, 3f), 5f, 0f);

    }
}


//We need to anchor our spring to a fixed joint
public class fixedJoint
{
    public GameObject fixedJointObject;
    public Rigidbody body;

    public fixedJoint(Vector3 location)
    {
        fixedJointObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fixedJointObject.transform.position = location;

        //We need to create a new material for WebGL
        Renderer r = fixedJointObject.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));

        FixedJoint fJoint = fixedJointObject.AddComponent<FixedJoint>();
        body = fixedJointObject.GetComponent<Rigidbody>();
    }

}


//This will contain our spring joint information
public class springJoint
{
    public GameObject springJointObject;
    public SpringJoint sJoint;

    public springJoint(Vector3 location)
    {
        springJointObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        springJointObject.transform.position = location;

        //We need to create a new material for WebGL
        Renderer r = springJointObject.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));

        sJoint = springJointObject.AddComponent<SpringJoint>();
    }

}
