using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionFig3 : MonoBehaviour
{
    //And then we need to be able to access the walker Component on our walkerGO (Walker Game Object)
    private WalkerIntro3 walkeri3;

    // Start is called before the first frame update
    void Start()
    {
        // Create the walker
        walkeri3 = new WalkerIntro3();
    }

    // Update is called once per frame
    void Update()
    {
        //Have the walker choose a direction
        walkeri3.step();
        //Instantiate the sphere in the last previous location to "draw" the path
        walkeri3.draw();
    }
}

public class WalkerIntro3 
{
    public int x;
    public int y;
    float num;
    GameObject walkerGO;
    List<GameObject> walkers = new List<GameObject>();

    // Start is called before the first frame update
    public WalkerIntro3 ()
    {
        walkerGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);     //We need to create a new material for WebGL
        Renderer r = walkerGO.GetComponent<Renderer>();
        walkerGO.GetComponent<SphereCollider>().enabled = false;
        Object.Destroy(walkerGO.GetComponent<SphereCollider>());
        r.material = new Material(Shader.Find("Diffuse"));

        x = 0;
        y = 0;
    }


    public void step()
    {
        num = Random.Range(0f, 1f);

        //Each frame choose a new Random number 0-1;
        //If the number is less than the the float take a step
        if (num < 0.2F)
        {
            y++;
        }
        else if (num > 0.2F && num < 0.4F)
        {
            y--;
        }
        else if (num > 0.4F && num < .6F)
        {
            x--;
        }
        else if (num > .6f)
        {
            x--;
        }
        walkerGO.transform.position = new Vector3(x, y, 0F) * Time.time;
    }

    //Now let's draw the path of the Mover by creating spheres in its position in the most recent frame.
    public void draw()
    {
 
        if (walkers.Count <= 100)
        {
            //This creates a sphere GameObject
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);     //We need to create a new material for WebGL
            Renderer r = sphere.GetComponent<Renderer>();
            sphere.GetComponent<SphereCollider>().enabled = false;
            Object.Destroy(sphere.GetComponent<SphereCollider>());
            r.material = new Material(Shader.Find("Diffuse"));
            //This sets our ink "sphere game objects" at the position of the Walker GameObject (walkerGO) at the current frame
            //to draw the path
            sphere.transform.position = new Vector3(walkerGO.transform.position.x, walkerGO.transform.position.y, 0F);
            walkers.Add(sphere);
        }
        else if (walkers.Count <= 1)
        {
            foreach(GameObject walker in walkers)
            {
                GameObject.Destroy(walker);
            }
        }
    }
}


