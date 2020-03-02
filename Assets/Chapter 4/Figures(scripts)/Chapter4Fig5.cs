using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter4Fig5 : MonoBehaviour
{
    public GameObject particleSystemCh4F3;
    Vector3 origin =  new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        createParticleSystem(origin);
    }

    void createParticleSystem(Vector3 origin)
    {
        particleSystemCh4F3 = Instantiate(particleSystemCh4F3);
        particleSystemChapter4Fig3 pSCh4F3 = particleSystemCh4F3.GetComponent<particleSystemChapter4Fig3>();
    }
}
