using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionFig5 : MonoBehaviour
{
    // We need to instantiate the Walker PrefAB
    public GameObject WalkerPrefab;
    //And create a variable to track it
    private GameObject walkerGO;
    //And then we need to be able to access the walker Component on our walkerGO (Walker Game Object)
    private WalkerIntro5 walkeri5;

    // Start is called before the first frame update
    void Start()
    {
        //Create an empty GameObject for the component we'll be adding.
        GameObject walkerGameObject = new GameObject();
        // Add the component
        walkeri5 = walkerGameObject.AddComponent<WalkerIntro5>();
    }

    // Update is called once per frame
    void Update()
    {
        //Have the walker move
        walkeri5.step();
    }

}



public class WalkerIntro5 : MonoBehaviour
{
    //GameObject
    float x, y;
    GameObject walkerGo;

    //Perlin
    float heightScale = 2;
    float widthScale = 1;

    float xScale, yScale;

    // Start is called before the first frame update
    void Start()
    {
        walkerGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    }

    // Update is called once per frame
    void Update()
    {
        widthScale += .02f;
        heightScale += .001f;
    }

    public void step()
    {
        float height = heightScale * Mathf.PerlinNoise(Time.time * .5f, 0.0f);
        float width = widthScale * Mathf.PerlinNoise(Time.time * 1, 0.0f);
        Vector3 pos = transform.position;
        pos.y = height;
        pos.x = width;
        walkerGo.transform.position = pos;
    }
}


