using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter5Fig9 : MonoBehaviour
{
    Vector2 minimumPos = new Vector2(-5, 0);
    fallingRocks rocks;

    [SerializeField]
    Collider slope;

    [SerializeField]
    int rockCount;

    // Start is called before the first frame update
    void Start()
    {
        PhysicMaterial pMaterial = new PhysicMaterial();
        //Create a bouncy material first
        pMaterial.bounciness = 1f;
        pMaterial.bounceCombine = PhysicMaterialCombine.Multiply;
        pMaterial.staticFriction = 0f;
        pMaterial.frictionCombine = PhysicMaterialCombine.Average;
        slope.material = pMaterial;

        for (int i = 0; i < rockCount; i++)
        {
            rocks = new fallingRocks(new Vector3(minimumPos.x, 10f, 7f));
            minimumPos.x = minimumPos.x + rocks.rock.transform.localScale.x;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            changeSlopeMaterial();
        }
    }

    

    public void changeSlopeMaterial()
    {
        PhysicMaterial pMaterial = new PhysicMaterial();
        //Create a bouncy material first
        pMaterial.bounciness = 0f;
        pMaterial.bounceCombine = PhysicMaterialCombine.Multiply;
        pMaterial.staticFriction = 100f;
        pMaterial.frictionCombine = PhysicMaterialCombine.Multiply;
        slope.material = pMaterial;

    }

}







public class fallingRocks
{

    public GameObject rock;
    
    public fallingRocks(Vector3 location)
    {
        rock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rock.transform.position = location;
        //We need to create a new material for WebGL
        Renderer r = rock.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));

        Rigidbody body = rock.AddComponent<Rigidbody>();

        // Generate random properties for this mover
        float radius = Random.Range(0.1f, 0.4f);

        // The default diameter of the sphere is one unit
        // This means we have to multiple the radius by two when scaling it up
        rock.transform.localScale = 2 * radius * Vector3.one;

        // We need to calculate the mass of the sphere.
        // Assuming the sphere is of even density throughout,
        // the mass will be proportional to the volume.
        body.mass = (4f / 3f) * Mathf.PI * radius * radius * radius;


    }


}