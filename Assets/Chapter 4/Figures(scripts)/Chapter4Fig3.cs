using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter4Fig3 : MonoBehaviour
{

    public GameObject particleSystemCh4F3;
    Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            origin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            createParticleSystem(origin);
        }
        Debug.Log(origin);

    }

    void createParticleSystem(Vector3 origin)
    {
        particleSystemCh4F3 = Instantiate(particleSystemCh4F3);
        particleSystemChapter4Fig3 pSCh4F3 = particleSystemCh4F3.GetComponent<particleSystemChapter4Fig3>();

        if (origin.x >= .5f ) {

            pSCh4F3.origin.x = origin.x * 2;

        } else if (origin.x < .5f ) {
            
            pSCh4F3.origin.x = origin.x * -2;

        }
        
        if (origin.y >= .5f)
        {

            pSCh4F3.origin.y = origin.y * 2;

        }
        else if (origin.y < .5f)
        {

            pSCh4F3.origin.y = origin.y * -2;

        }

    }

}
