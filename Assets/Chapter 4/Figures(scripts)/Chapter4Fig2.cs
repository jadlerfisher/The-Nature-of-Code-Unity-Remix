using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter4Fig2 : MonoBehaviour
{

    public particleChapter4_2 ps;
    public List<particleChapter4_2> particles = new List<particleChapter4_2>();
     

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(createParticle());
        IEnumerator<particleChapter4_2> it = particles.GetEnumerator();

        while (it.MoveNext())
        {
            particleChapter4_2 p = it.Current;
        if (p.isDead())
            {
               particles.Remove(p);
            }
        }
    }

    IEnumerator createParticle()
    {
        yield return new WaitForSeconds(2.0f);
        Instantiate(ps);
        particles.Add(ps);
    }
}
