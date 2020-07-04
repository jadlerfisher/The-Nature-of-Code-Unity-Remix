using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter5Fig6 : MonoBehaviour
{
    private ParticleSystem windParticles;
    public float windForce = 10.0f;

    void Start()
    {
        windParticles = GetComponent<ParticleSystem>();

        GameObject wind = new GameObject("Wind", typeof(WindZone));
        wind.transform.parent = windParticles.transform;
        wind.transform.localPosition = new Vector3(-5.0f, 0f, 0.0f);
        wind.GetComponent<WindZone>().mode = WindZoneMode.Spherical;
    }

    void Update()
    {
        var externalForces = windParticles.externalForces;
        externalForces.enabled = true;
        //While we cannot give the wind a force, we can assign an external force to the Wind from
        //the particle system
        externalForces.multiplier = windForce;
        //Now let's change how blustery it is
        StartCoroutine(changeWindForce());

    }

    IEnumerator changeWindForce()
    {
        //Print the time of when the function is first called.
        windForce = Random.Range(1f, 25f);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        windForce = Random.Range(25f, 100f);
    }
}
