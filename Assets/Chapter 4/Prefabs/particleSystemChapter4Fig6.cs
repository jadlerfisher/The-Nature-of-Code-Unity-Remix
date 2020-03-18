using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemChapter4Fig6 : MonoBehaviour
{


    public GameObject ps;
    public List<particleChapter4_6> particles = new List<particleChapter4_6>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(createParticle());
        IEnumerator<particleChapter4_6> it = particles.GetEnumerator();

        while (it.MoveNext())
        {
            particleChapter4_6 p = it.Current;
            if (p.isDead())
            {
                particles.Remove(p);
            }
        }
    }

    IEnumerator createParticle()
    {
        yield return new WaitForSeconds(2.0f);
        Instantiate(ps, new Vector3(0f, 6f,0f), Quaternion.identity);
        particles.Add(ps.GetComponent<particleChapter4_6>());
    }

    public void applyForce(Vector3 force)
    {
        foreach (particleChapter4_6 particle in particles)
        {
            particle.applyForce(force);
        }
    }
}


