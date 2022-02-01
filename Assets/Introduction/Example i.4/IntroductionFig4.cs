using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionFig4 : MonoBehaviour
{
    public Material transparencyPrefab;

    // Update is called once per frame
    void FixedUpdate()
    {
        //To create a Gaussian distribution in Unity we can use Random.Range() within two separate Random.Range()
        float num = Random.Range(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        float sd = 30;
        float mean = 5;

        float x = sd * num + mean;

        //This creates a sphere GameObject and applies the transparency material prefab we created in Unity.
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer r = sphere.GetComponent<Renderer>();
        r.material = transparencyPrefab;

        //This instantiates the sphere with an X position calculated by our Gaussian distribution
        sphere.transform.position = new Vector3(x, 0F, 0F) * Time.deltaTime;
    }
    
}
 