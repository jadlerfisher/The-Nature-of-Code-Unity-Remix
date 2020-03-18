using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemChapter4Fig7 : MonoBehaviour
{

    public particleChapter4_6 particle;
    public List<particleChapter4_6> particles = new List<particleChapter4_6>();
    public Vector3 origin = new Vector3(0f, 6f, 0f);
    private float particleChange;

    public float mass = 1f;
    public Vector3 gravity = new Vector3(0f, -100f, 0f);
   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine(createParticle());
        particles.Clear();
    }

    IEnumerator createParticle()
    {
        yield return new WaitForSeconds(2.0f);
        particleChapter4_6 p = Instantiate(particle, origin, Quaternion.identity);
        if (p.gameObject.activeInHierarchy)
        {
            particles.Add(p);
        }
        else 
        {
            Debug.Log("error");
        }
    }

    public void applyForce(Vector3 force)
    {
        foreach (particleChapter4_6 p in particles)
        {
            Vector3 f = force;
            f /= mass;
            p.acceleration += f;
        }
    }

    public void applyRepeller(repellerChapter4Fig7 r)
    {
        foreach (particleChapter4_6 p in particles)
        {
            Vector3 force = r.repel(p);
            p.applyForce(force);
        }

    }
}


