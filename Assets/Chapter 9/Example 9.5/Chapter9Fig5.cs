using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter9Fig5 : MonoBehaviour
{
    [SerializeField] int num = 10; // Number of Bloops
    [SerializeField] float foodSpawnRate = 1; // The rate (in seconds) which food spawns
    private Vector2 maximumPos;

    void Start()
    {
        FindWindowLimits();

        for (int i = 0; i < num; i++) 
        {
            GameObject bloopObj = GameObject.CreatePrimitive(PrimitiveType.Sphere); // Make an object
            Bloop bloop = bloopObj.AddComponent<Bloop>(); // Turn it into a bloop!
        }

        for (int i = 0; i < num*3; i++) 
        {
            SpawnFood(); // We'll package food spawning into a function so we can reuse the code!
        }

        StartCoroutine(FoodSpawner());
    }

    private IEnumerator FoodSpawner() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(foodSpawnRate);
            SpawnFood();
        }
    }

    private void SpawnFood() 
    {
        GameObject fd = GameObject.CreatePrimitive(PrimitiveType.Cube);
        BoxCollider bx = fd.AddComponent<BoxCollider>();
        MeshRenderer r = fd.GetComponent<MeshRenderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        r.material.color = Color.grey;
        fd.transform.localScale *= 0.3f; // Let's make the object small
        fd.transform.position = new Vector2(Random.Range(-maximumPos.x, maximumPos.x), Random.Range(-maximumPos.y, maximumPos.y)); // Set a random location for the food
        fd.layer = 9; // We'll set the object to it's own layer so we can find it much easier later
        fd.name = "Food"; // We'll name it too!
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // Set the desired camera size
        Camera.main.orthographicSize = 10;

        // Set position of the camera to ensure 0,0 x,y
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}