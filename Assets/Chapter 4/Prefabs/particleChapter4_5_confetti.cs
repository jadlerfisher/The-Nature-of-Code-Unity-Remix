using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleChapter4_5_confetti : particleChapter4_5
{

    void Start()
    {
        particleMeshRenderer = this.GetComponent<MeshRenderer>();
        velocity = new Vector3(Random.Range(-.5f, .5f), Random.Range(-1f, 0f), 0f);
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

            lifespan = lifespan - Random.Range(.001f, .06f);

            Color col = particleMeshRenderer.material.GetColor("_Color");

            particleMeshRenderer.material.color = new Color(col.r, col.g, col.b, lifespan);
        }
        else
        {
            Debug.Log("le mort");
        }
    }
}
