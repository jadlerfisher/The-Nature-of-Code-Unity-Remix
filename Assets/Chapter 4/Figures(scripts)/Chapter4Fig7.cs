using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter4Fig7 : MonoBehaviour
{
    public GameObject particleSystemCh4F7;
    particleSystemChapter4Fig7 pSCh4F7;
    Vector3 origin;

    public GameObject repeller;
    private repellerChapter4Fig7 rC4F7;

    // Start is called before the first frame update
    void Start()
    {
        repeller = Instantiate(repeller);
        repeller.transform.position = new Vector3(0f, -4f, 0f);
        rC4F7 = repeller.GetComponent<repellerChapter4Fig7>();
        createParticleSystem(origin);
    }

    void createParticleSystem(Vector3 origin)
    {
        particleSystemCh4F7 = Instantiate(particleSystemCh4F7);
        pSCh4F7 = particleSystemCh4F7.GetComponent<particleSystemChapter4Fig7>();
    }

    void Update()
    {

        Vector3 gravity = new Vector3(0f, -.0001f, 0f);
        pSCh4F7.applyForce(gravity);
        pSCh4F7.applyRepeller(rC4F7);
    }
}
