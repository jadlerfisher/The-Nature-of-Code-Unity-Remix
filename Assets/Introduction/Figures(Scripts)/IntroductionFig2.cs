using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionFig2 : MonoBehaviour
{
    //Because we are in C# and will be modifying the length and items of these values at runtime we need to use a list instead of an array
    //Read more here: docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/

   public List<int> randomCount = new List<int>();
   public List<GameObject> cubes = new List<GameObject>();
   public GameObject cubeGo;

// Set a fake creeen height for Unity
   int height = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Grab a random number betwen -1 and the number of values in our random number list
            int index = Random.Range(-1, randomCount.Count);
        //if the number does not exist in the list, add it
            if (!randomCount.Contains(index))
            {
                randomCount.Add(index);
            }

            //Create a fake width to maintain our cubes in a reasonable 3D space
            int w = 30 / randomCount.Count;

        //For each of the random values, up until 100 (for memory) create a cube and place it at a new Vector
            for (int x = 0; x < randomCount.Count; x++)
            {
                if (randomCount.Count < 100)
                {
                GameObject cube = Instantiate(cubeGo);
                //This new vector is created by multiplying the cube's index (an int) and multiplying it by the fake width of the screen. 
                //The Y coordinate is chosen by subtracting the cube's randomly chosen int and subtracting it from the height. In this case 0.
                cube.transform.position = new Vector3(w * x, (height-randomCount[x])/10, 0F);
        //We then add the cube to our other list so we can deal with the scale.
                    cubes.Add(cube);
                }
            for (int y = 0; y < cubes.Count; y++)
            {
                //Now for each of the cubes, we will scale the Y value, its height, by choosing a random value from the list randomCount
                cubes[y].transform.localScale = new Vector3(1F, (randomCount[Random.Range(0, randomCount.Count)]), 1F);
            }
        }

    }
}
