using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Chapter4Fig2 : MonoBehaviour
{
    Vector3 origin;
    particleSystemFigure2 psf2;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            createParticleSystem(origin);
        } else if (Input.GetMouseButtonDown(1))
        {
            origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            createParticleSystem(origin, psf2.particleSystemGameObject);
        }
    }

    void createParticleSystem(Vector3 origin)
    {
        Vector3 particleSystemLocation = new Vector3(origin.x, origin.y, -10);
        Vector3 velocity = new Vector3(1f, 3f, 0f);
        float lifeTime = 5f;
        float startSpeed = Random.Range(-3f, 0f);
        int maxParticles = 1000;

        psf2 = new particleSystemFigure2(particleSystemLocation, startSpeed, velocity, lifeTime, maxParticles);
    }

    void createParticleSystem(Vector3 origin, GameObject parentParticleSystem)
    {
        Vector3 particleSystemLocation = new Vector3(origin.x, origin.y, -10);
        Vector3 velocity = new Vector3(1f, 3f, 0f);
        float lifeTime = 5f;
        float startSpeed = Random.Range(-3f, 0f);
        int maxParticles = 1000;
        //Create the child particle system
        particleSystemFigure2 child = new particleSystemFigure2(particleSystemLocation, startSpeed, velocity, lifeTime, maxParticles);
        //Now let's go ahead and give the child a parent particle system
        child.particleSystemGameObject.transform.SetParent(psf2.particleSystemGameObject.transform);

        //Now let's turn on the inherit velocity module
        child.inheritVelocityModule();
    }
}

public class particleSystemFigure2
{
    //We need to create a GameObject to hold the ParticleSystem component
    public GameObject particleSystemGameObject;

    //This is the ParticleSystem component but we'll need to access everything through the .main property
    //This is because ParticleSystems in Unity are interfaces and not independent objects
    ParticleSystem particleSystemComponent;

    public particleSystemFigure2(Vector3 particleSystemLocation, float startSpeed, Vector3 velocity, float lifeTime, int maxParticles)
    {
        //Create the GameObject in the constructor
        particleSystemGameObject = new GameObject();
        //Move the GameObject to the right position
        particleSystemGameObject.transform.position = particleSystemLocation;
        //Add the particle system
        particleSystemComponent = particleSystemGameObject.AddComponent<ParticleSystem>();

        //Now we need to gather the interfaces of our ParticleSystem
        //The main interface covers general properties
        var main = particleSystemComponent.main;

        //In the Main Interface we'll sat the initial start LifeTime (how long a single particle will live)
        //And, of course, we'll set our Max Particles
        main.startLifetime = lifeTime;
        main.startSpeed = startSpeed;
        main.maxParticles = maxParticles;

        //Now we'll call methods to control the velocity of individual particles and their colors
        velocityModule(velocity);
        colorModule();
    }

    public void inheritVelocityModule()
    {
        var inheritVelocityModule = particleSystemComponent.inheritVelocity;
        inheritVelocityModule.enabled = true;
        //Let's change the color of the child material so it is easier to see
        ParticleSystemRenderer r = particleSystemGameObject.GetComponent<ParticleSystemRenderer>();
        r.material.color = Color.red;

        //Now let's grab the current velocity from the parent particleSystem 
        inheritVelocityModule.mode = ParticleSystemInheritVelocityMode.Current;
        //And add a multiplier so they move faster
        //We can use a curve, like above in velocity, or just a general multiplier.
        inheritVelocityModule.curveMultiplier = 100;
    }

    public void velocityModule(Vector3 velocity)
    {
        //The velocityOverLifetime inferface controls the velocity of individual particles
        var velocityOverLifetime = particleSystemComponent.velocityOverLifetime;

        //First we need to enable the Velocity Over Lifetime Interface;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;

        //We then to create a MinMaxCurves which will manage the change in velocity a
        ParticleSystem.MinMaxCurve minMaxCurveX = new ParticleSystem.MinMaxCurve(-velocity.x * velocity.x, velocity.x);
        ParticleSystem.MinMaxCurve minMaxCurveY = new ParticleSystem.MinMaxCurve(-velocity.y * velocity.y, -velocity.y);

        velocityOverLifetime.x = minMaxCurveX;
        velocityOverLifetime.y = minMaxCurveY;
        //Even though we are not using Z, Unity needs us to otherwise it will throw an error. 
        //This is a bug in 2019.
        velocityOverLifetime.z = minMaxCurveY;
    }

    public void colorModule()
    {
        //The colorOverLifetime interfaces manages the color of the objects over their lifetime.
        var colorOverLifetime = particleSystemComponent.colorOverLifetime;

        //While we are here, let's add a material to our particles
        ParticleSystemRenderer r = particleSystemGameObject.GetComponent<ParticleSystemRenderer>();
        //There a few different ways to do this, but we've created a material that is based on the default particle shader
        r.material = Resources.Load<Material>("particleMaterial");

        //To have the particle become transparent we need to access the colorOverLifetime Interface
        colorOverLifetime.enabled = true;
        Gradient grad = new Gradient();
        //This gradient key lets us choose points on a gradient that represent different RGBA or Unity.Color values.
        //These gradient values exist in an array
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0f, 2.0f) });
        //Set the color to the gradient we created above
        colorOverLifetime.color = grad;
    }

}