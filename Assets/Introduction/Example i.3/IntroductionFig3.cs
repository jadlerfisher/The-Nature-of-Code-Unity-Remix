using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionFig3 : MonoBehaviour
{
    private IntroMover mover;

    // Start is called before the first frame update
    void Start()
    {
        //Create the walker
        mover = new IntroMover();
    }

    // Update is called once per frame
    void Update()
    {
        //Have the walker choose a direction
        mover.Step();
        //Instantiate a sphere in the last previous location to "draw" the path
        mover.Draw();
    }
}

public class IntroMover 
{
    public int x;
    public int y;
    float num;
    GameObject moverGO;
    List<GameObject> drawnSpheres = new List<GameObject>();

    public IntroMover ()
    {
        moverGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);     
        //We need to create a new material for WebGL
        Renderer r = moverGO.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));

        x = 0;
        y = 0;
    }


    public void Step()
    {
        //Each frame choose a new Random number 0-1;
        num = Random.Range(0f, 1f);
        
        //The increment direction is decided by our custom probabilities
        if (num < 0.2f) //20% chance to move up
        {
            y++;
        }
        else if (num > 0.2f && num < 0.4f) //20% chance to move down
        {
            y--;
        }
        else if (num > 0.4f && num < .6f) //20% chance to move left
        {
            x--;
        }
        else if (num > .6f) //40% chance to move right
        {
            x++;
        }
        moverGO.transform.position = new Vector3(x, y, 0f) * Time.time;
    }

    //Now let's draw the path of the Mover by creating spheres in its position in the most recent frame.
    public void Draw()
    {
        if (drawnSpheres.Count < 200)
        {
            //This creates a sphere GameObject
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //We need to create a new material for WebGL
            Renderer r = sphere.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Diffuse"));
            //This sets our sphere game objects at the position of the mover GameObject (moverGO) at the current frame to draw the path.
            sphere.transform.position = new Vector3(moverGO.transform.position.x, moverGO.transform.position.y, 0F);
            drawnSpheres.Add(sphere);
        }
    }
}


