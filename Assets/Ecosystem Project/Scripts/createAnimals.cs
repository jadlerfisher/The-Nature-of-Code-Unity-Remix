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




    public GameObject hawk;
    public int hawkTotal;

    public GameObject bear;
    public int bearTotal;


    public perlinTerrain terrain;


    /// <summary>
    /// Animal Food
    /// </summary>

    public List<GameObject> redBirdList = new List<GameObject>();

    public GameObject redBird;
    public int redBirdTotal;
    Vector3 redBirdStart;


    public List<GameObject> food = new List<GameObject>();

    public GameObject seed;
    public int seedTotal;
    Vector3 seedStart;

    public List<GameObject> berryList = new List<GameObject>();

    public GameObject berry;
    public int berryTotal;
    Vector3 beeryStart;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < flyTotal; i++)
        {
          
            fly = Instantiate(fly, new Vector3(Random.Range(10f, terrain.cols), Random.Range(4f, 20f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            Insects.Add(fly);

        }

        for (int j = 0; j < antTotal; j++)
        {

            ant = Instantiate(ant, new Vector3(Random.Range(10f, terrain.cols), Random.Range(5f, 7f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            Insects.Add(ant);

        }

        for (int j = 0; j < blueBirdTotal; j++)
        {

            blueBird = Instantiate(blueBird, new Vector3(Random.Range(10f, terrain.cols), Random.Range(6f, 15f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            Birds.Add(blueBird);

        }

        for (int j = 0; j < redBirdTotal; j++)
        {

            GameObject redB = Instantiate(redBird, new Vector3(Random.Range(10f, terrain.cols), Random.Range(10f, 15f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            redBirdList.Add(redB);

        }

        for (int j = 0; j < seedTotal; j++)
        {

            GameObject see = Instantiate(seed, new Vector3(Random.Range(10f, terrain.cols), Random.Range(8f, 10f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            food.Add(see);

        }

        for (int j = 0; j < berryTotal; j++)
        {

            GameObject berr = Instantiate(berry, new Vector3(Random.Range(10f, terrain.cols), Random.Range(5f, 6f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            berryList.Add(berr);

        }

        for (int j = 0; j < hawkTotal; j++)
        {

            hawk = Instantiate(hawk, new Vector3(Random.Range(10f, terrain.cols), Random.Range(10f, 20f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            Birds.Add(hawk);

        }
        for (int j = 0; j < bearTotal; j++)
        {

            GameObject b = Instantiate(bear, new Vector3(Random.Range(10f, terrain.cols), Random.Range(10f, 11f), Random.Range(10f, terrain.rows)), Quaternion.identity);
            Birds.Add(b);

        }
    }

    // Update is called once per frame
    void Update()
    {
       if (redBirdList.Count <= 50)
        {

            StartCoroutine(circleOfLife(redBird, new Vector3(Random.Range(10f, terrain.cols), Random.Range(10f, 15f), Random.Range(10f, terrain.rows))));

        }
        
         if (seedTotal <= 10)
        {
            StartCoroutine(circleOfLife(seed, new Vector3(Random.Range(10f, terrain.cols), Random.Range(5f, 8f), Random.Range(10f, terrain.rows))));

        }
        
        if (berryList.Count <= 50)
        {
            StartCoroutine(circleOfLife(berry, new Vector3(Random.Range(10f, terrain.cols), Random.Range(5f, 6f), Random.Range(10f, terrain.rows))));


        }


    }


    IEnumerator circleOfLife(GameObject prey, Vector3 position)
    {
        yield return new WaitForSeconds(10);
        if (prey.name == "redbird(Clone)" || prey.name == "redbird")
        {
            if (redBirdList.Count <= redBirdTotal) {
                GameObject b = Instantiate(prey, position, Quaternion.identity);
                redBirdList.Add(b);
            }
            
        }
        else if (prey.name == "berry(Clone)" || prey.name == "berry") 
        {
            if (berryList.Count <= berryTotal)
            {
                GameObject c = Instantiate(prey, position, Quaternion.identity);
                if (c.name == "berry" || c.name == "berry(Clone)")
                {
                    berryList.Add(c);
                }
                else {
                    Destroy(c);
                }
            }
        }
    }

}
