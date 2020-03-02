using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemChapter4Fig5 : MonoBehaviour
{

    public particleChapter4_5 psOriginal;
    public particleChapter4_5 psConfetti;
    public List<particleChapter4_5> particles = new List<particleChapter4_5>();
    public Vector3 origin;
    private float particleChange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(createParticle());

        for (int i = 0; i < particles.Count; i++)
        {
            if (particles[i].isDead())
            {
                particles.Remove(particles[i]);
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
        particleChange = Random.Range(0f, 1f);
        if (particleChange < .5f)
        {
            particleChapter4_5 p = Instantiate(psOriginal, origin, Quaternion.identity);
            particles.Add(p);
        }
        else 
        {
            particleChapter4_5 p = Instantiate(psConfetti, origin, Quaternion.identity);
            particles.Add(p);
        }
    }

}


