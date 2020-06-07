using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Chapter4Fig6 : MonoBehaviour
{
    particleSystemFigure6 psf6;
    Vector3 particleSystemLocation = new Vector3(0, 2f, 0);
    public float lifeTime;
    public float startSpeed;

    int maxParticles;
    
    public GameObject collisionPlane1, collisionPlane2;

    // Start is called before the first frame update
    void Start()
    {
        //Let's just have one particle
        maxParticles = 1000;
        psf6 = new particleSystemFigure6(particleSystemLocation, startSpeed, lifeTime, maxParticles, collisionPlane1, collisionPlane2);
    }

}

public class particleSystemFigure6
{
    //We need to create a GameObject to hold the ParticleSystem component
    GameObject particleSystemGameObject;

    //This is the ParticleSystem component but we'll need to access everything through the .main property
    //This is because ParticleSystems in Unity are interfaces and not independent objects
    ParticleSystem particleSystemComponent;

    ParticleSystemRenderer r;

    GameObject collisionPlane1, collisionPlane2;

    public particleSystemFigure6(Vector3 particleSystemLocation, float startSpeed, float lifeTime, int maxParticles, GameObject collisionPlane1, GameObject collisionPlane2)
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

        //Now we can simply turn gravity on
        //Physics.gravity is already set to 9.8 but you can modify
        //this at anytime
        main.gravityModifier = 1f;

        //Add our collission planes so we can access them later
        this.collisionPlane1 = collisionPlane1;
        this.collisionPlane2 = collisionPlane2;
        collisionModule(collisionPlane1, collisionPlane2);

        colorModule();
    }

    public void collisionModule(GameObject plane1, GameObject plane2)
    {
        var collisionModule = particleSystemComponent.collision;
        collisionModule.enabled = true;
        //Now we can collide with items in world space or planes.
        collisionModule.type = ParticleSystemCollisionType.Planes;
        collisionModule.mode = ParticleSystemCollisionMode.Collision3D;
        //Since we have no world, we'll add two planes for the particles to collide against
        collisionModule.SetPlane(0, plane1.transform);
        collisionModule.SetPlane(1, plane2.transform);
        //The collision detection on the planes is quite large and can be reduced
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
