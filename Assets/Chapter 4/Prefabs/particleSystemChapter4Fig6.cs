using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemChapter4Fig6 : MonoBehaviour
{

    public particleChapter4_6 particle;
    public List<particleChapter4_6> particles = new List<particleChapter4_6>();
    public Vector3 origin;
    private float particleChange;

    public float mass = 1f;
    public Vector3 gravity = new Vector3(0f, -1f, 0f);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(createParticle());

        for (int i = 1; i < particles.Count; i++)
        {

            applyForce(particles[i], gravity);

            if (particles[i].isDead())
            {
                particles.Remove(particles[i]);
            }
            else 
            {
                applyForce(particles[i], particles[i].velocity);
        
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
        Debug.Log(f);
        f /= mass;
        pC46.acceleration += f;
    }
}


