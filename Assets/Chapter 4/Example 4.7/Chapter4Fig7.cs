using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Chapter4Fig7 : MonoBehaviour
{
    particleSystemFigure7 psf7;
    repeller r;
    Vector3 particleSystemLocation = new Vector3(0, 2f, 0);
    public float lifeTime;
    public float startSpeed;

    int maxParticles;

    // Start is called before the first frame update
    void Start()
    {
        //Let's just have one particle
        maxParticles = 1000;
        r = new repeller();
        psf7 = new particleSystemFigure7(particleSystemLocation, startSpeed, lifeTime, maxParticles, r.repelField);
    }

}

public class particleSystemFigure7
{
    //We need to create a GameObject to hold the ParticleSystem component
    GameObject particleSystemGameObject;

    //This is the ParticleSystem component but we'll need to access everything through the .main property
    //This is because ParticleSystems in Unity are interfaces and not independent objects
    ParticleSystem particleSystemComponent;

    ParticleSystemRenderer r;
    ParticleSystemForceField repeller;
    public particleSystemFigure7(Vector3 particleSystemLocation, float startSpeed, float lifeTime, int maxParticles, ParticleSystemForceField repeller)
    {
        //Create the GameObject in the constructor
        particleSystemGameObject = new GameObject();
        //Move the GameObject to the right position
        particleSystemGameObject.transform.position = particleSystemLocation;
        //Add the particle system
        particleSystemComponent = particleSystemGameObject.AddComponent<ParticleSystem>();
        this.repeller = repeller;

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
        main.gravityModifier = 1f;

        //Let's add external forces
        externalForceModule();
        colorModule();
    }

    public void externalForceModule()
    { 
        //Now we need to turn on the External Forces Module
        var externalForces = particleSystemComponent.externalForces;
        externalForces.enabled = true;

        //And now we just add the influential force of the repeller
        externalForces.AddInfluence(repeller);
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


public class repeller
{
    public Rigidbody body;
    private GameObject gameObject;
    public ParticleSystemForceField repelField;
    float repellerStrength = 10;

    public repeller()
    {
        // Create the components required for the mover
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.transform.position = new Vector3(0, -6f, 0);
        body = gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(gameObject.GetComponent<SphereCollider>());

        //We need to create a new material for WebGL
        Renderer r = body.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        r.material.color = Color.red;

        //Turn off gravity for this object
        body.useGravity = false;

        //Now let's add our Particle System Force Field Component
        repelField = gameObject.AddComponent<ParticleSystemForceField>();
        repelField.shape = ParticleSystemForceFieldShape.Sphere;
        //To repel the object away. We make the strength negative because unity doesn't differentiate the direction of a gravitational force
        //in the way we are doing it
        repelField.gravity = -repellerStrength;
        //We set a range for the field
        repelField.endRange = 2 * repelField.transform.localScale.x;
    }
}
