using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleChapter4_Base : MonoBehaviour
{
    public Vector3 location;
    public Vector3 velocity;
    public Vector3 acceleration;
    public float lifespan;

    public MeshRenderer particleMeshRenderer;

    public particleChapter4_Base()
    {
        location = new Vector3(0f, 6f, 0f);
        velocity = new Vector3(0f, 0f, 0f);
        acceleration = new Vector3(0f, 0f, 0f);
        lifespan = 1;
    }

        public particleChapter4_Base(Vector3 location)
    {
        acceleration = new Vector3(Random.Range(-.1f, .1f), Random.Range(-.2f, 0f), 0f);
        lifespan = 1;
    }

    void Start()
    {
        location = new Vector3(0f, 6f, 0f);
        acceleration = new Vector3(Random.Range(-.1f, .1f), Random.Range(-.2f, 0f), 0f);
    }

    // Update is called once per frame
    void Update()
    {

        if (!isDead())
        {
            velocity += acceleration;
            location += velocity;
            this.gameObject.transform.Translate(location * Time.deltaTime, Space.World);
            lifespan = lifespan - .02f;

            Color col = particleMeshRenderer.material.GetColor("_Color");

            particleMeshRenderer.material.color = new Color(col.r, col.g, col.b, lifespan);
        }
        else
        {
            //Do nothing
        }
    }

    public bool isDead()
    {
        if (lifespan < 0.0)
        {
            Destroy(gameObject);
            Destroy(this);
            return true;
        }
        else
        {
            return false;
        }
    }







}
