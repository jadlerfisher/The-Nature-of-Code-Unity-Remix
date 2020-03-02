using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemChapter4Fig7 : MonoBehaviour
{

    public particleChapter4_6 particle;
    public List<particleChapter4_6> particles = new List<particleChapter4_6>();
    public Vector3 origin;
    private float particleChange;

    public float mass = 1f;
    public Vector3 gravity = new Vector3(0f, -.000001f, 0f);
    public GameObject repeller;
    private repellerChapter4Fig7 rC4F7;

    // Start is called before the first frame update
    void Start()
    {
        repeller = Instantiate(repeller);
        repeller.transform.position = new Vector3(0f, -4f, 0f);
        rC4F7 = repeller.GetComponent<repellerChapter4Fig7>();
    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine(createParticle());

        for (int i = 0; i < particles.Count; i++)
        {
            applyForce(particles[i], gravity);
            applyRepeller(particles[i], rC4F7);


            if (particles[i].isDead())
            {
                particles.Remove(particles[i]);
            }
            else
            {
                applyRepeller(particles[i], rC4F7);

                //applyForce(particles[i], particles[i].velocity); 
            }

            for (int p = particles.Count; p >= 30; p--)
            {
                particles.Clear();
            }
        }
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

    public void applyForce(particleChapter4_6 pC46, Vector3 force)
    {
        Vector3 f = force;
        f /= mass;
        pC46.acceleration += f;
    }

    public void applyRepeller(particleChapter4_6 p, repellerChapter4Fig7 r)
    {
        Vector3 force = r.repel(p);
        applyForce(p, force);
        Debug.Log(force);

    }
}


