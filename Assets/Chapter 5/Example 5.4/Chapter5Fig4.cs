using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chapter5Fig4 : MonoBehaviour
{
    [SerializeField]
    //We can only include, at max, 32 capsule colliders
    [Range (1,32)]
    int clothColliderCount;
    clothColliders cC;
    //cloth Collider list
    List<CapsuleCollider> clothCapsuleColliders = new List<CapsuleCollider>();
    //The array of cloth colliders we'll pass to the cloth
    CapsuleCollider[] clothColliders;
    //Our cloth object for reference
    clothObject cloth;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        while (i < clothColliderCount)
        {
            i++;

            cC = new clothColliders();
            clothCapsuleColliders.Add(cC.capsuleCollider);

            if (i >= clothColliderCount)
            {
                //Once we have all of the capsules in the scene
                //We'll convert them to an array
                clothColliders = clothCapsuleColliders.ToArray();
                //Then instantiate our cloth object and pass along our colliders
                cloth = new clothObject(clothColliders);
            }
        }

        //Let's also change the gravity that impacts cloth in the scene
        //Physics.clothGravity = new Vector3(0f, .01f, 0f);
        Physics.gravity = new Vector3(0f, .1f, 0f);
    }

}



public class clothObject
{
    Cloth clothComponent;
    public GameObject clothGO;
    SkinnedMeshRenderer clothRenderer;
    public clothObject(CapsuleCollider[] clothColliders)
    {
        //Create a GameObject for the Cloth Component
        clothGO = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //Now, let's add our cloth component
        clothComponent = clothGO.AddComponent<Cloth>();
        //Let's make a big cloth
        clothGO.transform.localScale = new Vector3(4f, 4f, 4f);
        //And put it above the capsules
        clothGO.transform.localPosition = new Vector3(0f, 10f, 10f);
        //Make it red
        clothRenderer = clothGO.GetComponent<SkinnedMeshRenderer>();
        clothRenderer.material = new Material(Shader.Find("StandardDoubleSide"));
        clothRenderer.material.color = Color.red;

        //Turn on gravity
        clothComponent.useGravity = true;

        //We'll slow down the world acceleration and velocity
        clothComponent.worldVelocityScale = 1f;
        clothComponent.worldAccelerationScale = .3f;

        //We'll make our cloh stiff so it doesn't flutter too much
        clothComponent.bendingStiffness = .5f;
        clothComponent.damping = .3f;

        //Increase the friction of the cloth as well
        clothComponent.friction = 1f;

        //Add our colliders
        clothComponent.capsuleColliders = clothColliders;
    }
}


public class clothColliders
{
    //Basic capsule properties
    GameObject capsule;
    public CapsuleCollider capsuleCollider;
    Rigidbody body;
    Transform transform;

    public clothColliders()
    {
        capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        transform = capsule.transform;
        //Now let's place them at random places in the scene
        transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-6f, 6f), Random.Range(0f, 20f));
        //Let's make asure all of the capsules are a different size
        transform.localScale = new Vector3(Random.Range(1f, 4f), 1f, Random.Range(1f,4f));

        Renderer renderer = capsule.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Diffuse"));
        renderer.material.color = Color.white;

        //For the cloth to hit these capsules we need to add a collider
        capsuleCollider = capsule.GetComponent<CapsuleCollider>();
        //We have to create Bounds first
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        //Then we have to situate these bounds over the object
        //Our renderer helpfully provides some for us
        capsuleCollider.bounds.Encapsulate(renderer.bounds);
        capsuleCollider.center = renderer.bounds.center - transform.position;
        capsuleCollider.radius = 1f;
    }
}
