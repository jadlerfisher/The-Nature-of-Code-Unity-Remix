using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class particleChapter4_1 : MonoBehaviour
{

    public Vector3 location = new Vector3(0f, 6f, 0f);
    public Vector3 velocity = new Vector3(0f, 0f, 0f);
    public Vector3 acceleration = new Vector3(0f, -.05f, 0f);
    public float lifespan = 1;

    MeshRenderer particleMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        particleMeshRenderer = this.GetComponent<MeshRenderer>();
        velocity = new Vector3(Random.Range(-.1f, .1f), Random.Range(-.2f, 0f), 0f);
        float lifespan = 1;
        location = new Vector3(0f, 6f, 0f);
        this.transform.position = location;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isDead())
        {

            velocity += acceleration;
            location += velocity;
            this.transform.position = location;

            lifespan = lifespan - .02f;


            Color col = particleMeshRenderer.material.GetColor("_Color");

            particleMeshRenderer.material.color = new Color(col.r, col.g, col.b, lifespan);
            Debug.Log(particleMeshRenderer.material.color);
        } else
        {
            Debug.Log("le mort");
        }



    }

    public bool isDead()
    {

        if (lifespan < 0.0)
        {
            Destroy(gameObject);

            return true;
        }
        else
        {
            return false;
        }


    }
}
