using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemChapter4Fig4 : MonoBehaviour
{

    public particleChapter4_3 ps;
    public List<particleChapter4_3> particles = new List<particleChapter4_3>();
    public Vector3 origin;

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
        Instantiate(ps, origin, Quaternion.identity);
        particles.Add(ps);
    }

}

