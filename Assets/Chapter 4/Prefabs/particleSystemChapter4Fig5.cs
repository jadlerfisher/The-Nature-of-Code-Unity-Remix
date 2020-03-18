using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class particleSystemChapter4Fig5 : MonoBehaviour
{

    public List<GameObject> particles = new List<GameObject>();
    public Vector3 origin;
    public GameObject particle;
    public GameObject confetti;

    public particleSystemChapter4Fig5(Vector3 location)
    {
        origin = location;
        particles = new List<GameObject>();

    }

    void Start() {

    }

    public void addParticle()
    {
        origin = new Vector3(0f, 6f, 0f);
        float r = Random.Range(0f, 1f);
        if (r < 0.5f)
        {
            GameObject newP = Instantiate(particle, origin, Quaternion.identity);
            particles.Add(newP);
        }
        else
        {
            GameObject newC = Instantiate(confetti, origin, Quaternion.identity);
            particles.Add(newC);
        }
    }

    void Update()
    {
        StartCoroutine(createParticle());

        IEnumerator<GameObject> it = particles.GetEnumerator();

  

        while (it.MoveNext())
        {
            GameObject p = it.Current;
            if (p)
            {
                particleChapter4_Base pClass = p.GetComponent<particleChapter4_Base>();

                if (p.GetComponent<particleChapter4_5_confetti>())
                {
                    particleChapter4_5_confetti cClass = p.GetComponent<particleChapter4_5_confetti>();
                    cClass.rotate();
                }

                if (pClass.isDead())
                {
                    
                }
            }
        }

       particles.Clear();
    }

    IEnumerator createParticle()
    {
        yield return new WaitForSeconds(2.0f);
        addParticle();
    }
} 
   


