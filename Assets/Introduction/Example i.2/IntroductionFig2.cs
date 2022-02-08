using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionFig2 : MonoBehaviour
{
   List<int> randomCount = new List<int>();
   List<GameObject> cubes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        while(randomCount.Count < 40)
        {
            // Grab a random number betwen -1 and 40
            // We use 41 instead of 40 because Random.Range is max exclusive while using integer values
            int index = Random.Range(-1, 41);
            // if the number does not exist in the list, add it
            if (!randomCount.Contains(index))
            {
                randomCount.Add(index);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Create a fake width to maintain our cubes in a reasonable 3D space
        int w = 3;
        if (cubes.Count < randomCount.Count)
        {
            // For each of the random values, up until 40 (for memory) create a cube and place it at a new Vector
            for (int x = 0; x < randomCount.Count; x++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);     
                //We need to create a new material for WebGL
                Renderer r = cube.GetComponent<Renderer>();
                r.material = new Material(Shader.Find("Diffuse"));
                //The X coordinate is chosen by subtracting the cube's index x from the width w. In this case 3.
                cube.transform.position = new Vector3(w - x, 0f, 0f);
                cubes.Add(cube);
            }
        }

        foreach (GameObject cube in cubes)
        {
            int rand = randomCount[Random.Range(0, randomCount.Count)];
            // Now for each of the cubes, we will scale the Y value, its height, by choosing a random value from the list randomCount
            cube.transform.localScale += new Vector3(0f, rand, 0f) / 10 * Time.deltaTime;
        }
    }
}


