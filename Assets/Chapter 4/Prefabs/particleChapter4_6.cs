using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class particleChapter4_6 : MonoBehaviour
{
    public Vector3 location;
    public Vector3 velocity = new Vector3(0f, 0f, 0f);
    public Vector3 acceleration = new Vector3(0f, -.000000000005f, 0f);
    public float lifespan = 1;
    public float speed = 1.0f;
    public float mass = 1f;

    public MeshRenderer particleMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        particleMeshRenderer = this.GetComponent<MeshRenderer>();
        velocity = new Vector3(Random.Range(-.5f,.5f), 0f, 0f);
        float lifespan = 1;
        location = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
            if (!isDead())
            {
                velocity += acceleration;
                location += velocity;
                this.transform.position = location;

                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime; // calculate distance to move
                this.transform.position = Vector3.MoveTowards(transform.position, location, step);

                lifespan = lifespan - .01f;

                Color col = particleMeshRenderer.material.GetColor("_Color");

                particleMeshRenderer.material.color = new Color(col.r, col.g, col.b, lifespan);
            }
            else
            {
                Destroy(this.gameObject);
            }
    }

    public bool isDead()
    {
        if (lifespan < 0.0)
        {
            return true;
            Destroy(this);           
        }
        else
        {
            return false;
        }
    }

   
}
