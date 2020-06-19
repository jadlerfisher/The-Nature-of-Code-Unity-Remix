using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Chapter4Fig5 : MonoBehaviour
{
    particleSystemFigure5 psf5;
    Vector3 particleSystemLocation = new Vector3(0, 2f, 0);
    public float lifeTime;
    public float startSpeed;
    public Vector3 rotationVelocity;

    int maxParticles;

    // Start is called before the first frame update
    void Start()
    {
        //Let's just have one particle
        maxParticles = 1000;
        //For this example, let's pass 360-degree rotations into the Velocity
        rotationVelocity = new Vector3(360, 360, 0);
        psf5 = new particleSystemFigure5(particleSystemLocation, startSpeed, lifeTime, maxParticles, rotationVelocity);
    }

    void Update()
    {
        psf5.gravityModifier();
    }
}

public class particleSystemFigure5
{
    //We need to create a GameObject to hold the ParticleSystem component
    GameObject particleSystemGameObject;

    //This is the ParticleSystem component but we'll need to access everything through the .main property
    //This is because ParticleSystems in Unity are interfaces and not independent objects
    ParticleSystem particleSystemComponent;

    ParticleSystemRenderer r;

    public particleSystemFigure5(Vector3 particleSystemLocation, float startSpeed, float lifeTime, int maxParticles, Vector3 rotationVelocity)
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
        //The colorOverLifetime interfaces manages the color of the objects over their lifetime.
        var colorOverLifetime = particleSystemComponent.colorOverLifetime;


        //In the Main Interface we'll sat the initial start LifeTime (how long a single particle will live)
        //And, of course, we'll set our Max Particles
        main.startLifetime = lifeTime;
        main.startSpeed = startSpeed;
        main.maxParticles = maxParticles;

        //Now we can simply turn gravity on
        //Physics.gravity is already set to 9.8 but you can modify
        //this at anytime
        main.gravityModifier = 1f;

        colorModule();
        rotationOverLifetime(rotationVelocity);
        rotationBySpeed(rotationVelocity);
    }

    public void gravityModifier()
    {
        //The main interface covers general properties
        var main = particleSystemComponent.main;
        //We'll dramatically move gravity around to create a pulsing fountain effect
        main.gravityModifierMultiplier = Random.Range(-6, 10);
    }

    public void rotationOverLifetime(Vector3 rotationVelocity)
    {
        var rotationOverLifetime = particleSystemComponent.rotationOverLifetime;
        rotationOverLifetime.enabled = true;
        //we'll now go ahead and rotate 360-degrees on the x and y axes.
        rotationOverLifetime.separateAxes = true;
        //Now let's pass on the floats in our Vector3
        rotationOverLifetime.x = rotationVelocity.x;
        rotationOverLifetime.y = rotationVelocity.y;
        rotationOverLifetime.z = rotationVelocity.z;
    }

    public void rotationBySpeed(Vector3 rotationVelocity)
    {
        var rotationBySpeed = particleSystemComponent.rotationBySpeed;
        rotationBySpeed.enabled = true;
        //we'll now go ahead and rotate 360-degrees on the x and y axes.
        rotationBySpeed.separateAxes = true;
        //Now let's pass on the floats in our Vector3
        //This time let's pass the Y into the Z
        rotationBySpeed.x = 0f;
        rotationBySpeed.y = 0f;
        rotationBySpeed.z = rotationVelocity.y;
    }

    public void colorModule()
    {
        //The colorOverLifetime interfaces manages the color of the objects over their lifetime.
        var colorOverLifetime = particleSystemComponent.colorOverLifetime;
        colorOverLifetime.enabled = true;

        //While we are here, let's add a material to our particles
        ParticleSystemRenderer r = particleSystemGameObject.GetComponent<ParticleSystemRenderer>();
        //There a few different ways to do this, but we've created a material that is based on the default particle shader
        r.material = Resources.Load<Material>("particleMaterial");

        Gradient grad = new Gradient();
        //This gradient key lets us choose points on a gradient that represent different RGBA or Unity.Color values.
        //These gradient values exist in an array
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0f, 2.0f) });
        //Set the color to the gradient we created above
        colorOverLifetime.color = grad;
    }
}
