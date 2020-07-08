using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter9Fig5 : MonoBehaviour
{
    public int num = 10; // Number of Bloops
    public float foodSpawnRate = 1; // The rate (in seconds) which food spawns
    private Vector2 minPos, maxPos;

    void Start()
    {
        findWindowLimits();

        for (int i = 0; i < num; i++) 
        {
            GameObject bloopObj = GameObject.CreatePrimitive(PrimitiveType.Sphere); // Make an object
            Bloop bloop = bloopObj.AddComponent<Bloop>(); // Turn it into a bloop!
        }

        for (int i = 0; i < num*3; i++) 
        {
            spawnFood(); // We'll package food spawning into a function so we can reuse the code!
        }

        StartCoroutine(FoodSpawner());
    }

    private IEnumerator FoodSpawner() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(foodSpawnRate);
            spawnFood();
        }
    }

    private void spawnFood() 
    {
        GameObject fd = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer r = fd.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        fd.transform.localScale *= 0.3f; // Let's make the object small
        fd.transform.position = new Vector2(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y)); // Set a random location for the food
        fd.layer = 9; // We'll set the object to it's own layer so we can find it much easier later
        fd.name = "Food"; // We'll name it too!
    }


    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 5;
        minPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maxPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}