using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionFig2 : MonoBehaviour
{
   List<int> randomCount = new List<int>();
   List<GameObject> cubes = new List<GameObject>();

    // Set a fake creeen height for Unity
   int height = 0;

    // Start is called before the first frame update
    void Start()
    {
        while(randomCount.Count < 40)
        {
            //Grab a random number betwen -1 and the number of values in our random number list
            int index = Random.Range(-1, 40);
            //if the number does not exist in the list, add it
            if (!randomCount.Contains(index))
            {
                randomCount.Add(index);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Create a fake width to maintain our cubes in a reasonable 3D space
        int w = 4;
        if (cubes.Count < randomCount.Count)
        {
            //For each of the random values, up until 100 (for memory) create a cube and place it at a new Vector
            for (int x = 0; x < randomCount.Count; x++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);     //We need to create a new material for WebGL
                Renderer r = cube.GetComponent<Renderer>();
                cube.GetComponent<Collider>().enabled = false;
                Object.Destroy(gameObject.GetComponent<Collider>());
                r.material = new Material(Shader.Find("Diffuse"));
                //This new vector is created by multiplying the cube's index (an int) and multiplying it by the fake width of the screen. 
                //The Y coordinate is chosen by subtracting the cube's randomly chosen int and subtracting it from the height. In this case 0.
                cube.transform.position = new Vector3(w - x, 0f, 0F);
                cube.transform.localScale = new Vector3(1f, (height + randomCount[x]), 1F);
                cubes.Add(cube);
            }
        }

            foreach(GameObject cube in cubes)
            {
                float rando = randomCount[Random.Range(0, randomCount.Count)];
                //Now for each of the cubes, we will scale the Y value, its height, by choosing a random value from the list randomCount
                cube.transform.localScale += new Vector3(0f, rando, 0f) * Time.deltaTime;
                cube.transform.localPosition += (new Vector3(0f, rando, 0f)/100 * Time.deltaTime);
            }
    }
}


