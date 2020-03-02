using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter4Fig7 : MonoBehaviour
{
    public GameObject particleSystemCh4F7;
    Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        createParticleSystem(origin);
    }

    void createParticleSystem(Vector3 origin)
    {
        particleSystemCh4F7 = Instantiate(particleSystemCh4F7);
        particleSystemChapter4Fig7 pSCh4F7 = particleSystemCh4F7.GetComponent<particleSystemChapter4Fig7>();
    }
}
