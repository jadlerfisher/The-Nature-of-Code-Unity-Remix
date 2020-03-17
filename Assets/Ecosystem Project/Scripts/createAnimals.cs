using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createAnimals : MonoBehaviour
{

    //Insects

    public List<GameObject> Insects = new List<GameObject>();
    public GameObject fly;
    public int flyTotal;

    public GameObject ant;
    public int antTotal;


    //Birds
    public List<GameObject> Birds = new List<GameObject>();
    public GameObject blueBird;
    public int blueBirdTotal;


    public perlinTerrain terrain;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < flyTotal; i++)
        {
          
            fly = Instantiate(fly, new Vector3(Random.Range(10f, terrain.cols), Random.Range(2f, 6f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            Insects.Add(fly);

        }

        for (int j = 0; j < antTotal; j++)
        {

            ant = Instantiate(ant, new Vector3(Random.Range(10f, terrain.cols), Random.Range(3f, 5f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            Insects.Add(ant);

        }

        for (int j = 0; j < blueBirdTotal; j++)
        {

            blueBird = Instantiate(blueBird, new Vector3(Random.Range(10f, terrain.cols), Random.Range(6f, 15f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            Birds.Add(blueBird);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
