using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Chapter5Fig5 : MonoBehaviour
{
    Vector2 minimumPos, maximumPos;

    [SerializeField]
    int boulderCount;
    [SerializeField]
    float breakForce, breakTorque;

    boulders boulders;

    Vector3 particleSystemLocation = new Vector3(0, 4f, 0);
    public float lifeTime;
    public float startSpeed;

    avalanche avalanche;
    // Start is called before the first frame update
    void Start()
    {
        findWindowLimits();
        for (int i = 0; i < boulderCount; i++)
        {
            boulders = new boulders(new Vector3(minimumPos.x, -2f, 0), boulderCount, breakForce, breakTorque);
            minimumPos.x = minimumPos.x + boulders.boulder.transform.localScale.x+1f;
        }
        //Let's just have one particle
        int maxParticles = 1000;
        avalanche = new avalanche(particleSystemLocation, startSpeed, lifeTime, maxParticles, 10f);
        avalanche.particleSystemGameObject.transform.localRotation = Quaternion.EulerAngles(90f, 0f, 0f);

    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 8;
        minimumPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}


//This is a modification of Chapter4 Figure 6's particlesytem
public class avalanche{

    //We need to create a GameObject to hold the ParticleSystem component
    public GameObject particleSystemGameObject;

    //This is the ParticleSystem component but we'll need to access everything through the .main property
    //This is because ParticleSystems in Unity are interfaces and not independent objects
    ParticleSystem particleSystemComponent;

    ParticleSystemRenderer r;


    public avalanche(Vector3 particleSystemLocation, float startSpeed, float lifeTime, int maxParticles, float collisionForce)
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
        collisionModule(collisionForce);

        colorModule();
    }

    public void collisionModule(float collissionForce)
    {
        var collisionModule = particleSystemComponent.collision;
        collisionModule.enabled = true;
        //Now we can collide with items in world 
        collisionModule.type = ParticleSystemCollisionType.World;
        collisionModule.mode = ParticleSystemCollisionMode.Collision3D;
        //Now we add a collision force
        collisionModule.colliderForce = collissionForce;
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



public class boulders{

    public GameObject boulder;

    public boulders(Vector3 location, int boulderCount, float breakForce, float breakTorque)
    {
             boulder = GameObject.CreatePrimitive(PrimitiveType.Cube);
             boulder.transform.position = location;
             float scale = Random.Range(0f, 4f);
             boulder.transform.localScale = new Vector3(scale, scale, scale);
             //We need to create a new material for WebGL
             Renderer r = boulder.GetComponent<Renderer>();
             r.material = new Material(Shader.Find("Diffuse"));

            FixedJoint boulderJoint = boulder.AddComponent<FixedJoint>();
            boulderJoint.enableCollision = true;
            boulderJoint.enablePreprocessing = true;
            boulderJoint.breakForce = breakForce;
            boulderJoint.breakTorque = breakTorque;

            Rigidbody body = boulder.GetComponent<Rigidbody>();
            body.useGravity = false;
    }




}