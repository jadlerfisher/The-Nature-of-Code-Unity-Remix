using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the script that is going to control the entire ecosystem

public class ecosystem : MonoBehaviour
{

    //Chapter 1 Creature
    public List<GameObject> chapterOneCreatures = new List<GameObject>();
    public GameObject chapterOneCreature;
    public int chapterOneCreaturePopulation;

    //Chapter 2 Creature
    public List<GameObject> chapterTwoCreatures = new List<GameObject>();
    public GameObject chapterTwoCreature;
    public int chapterTwoCreaturePopulation;

    //Chapter 3 Creature
    public List<GameObject> chapterThreeCreatures = new List<GameObject>();
    public GameObject chapterThreeCreature;
    public int chapterThreeCreaturePopulation;

    //Chapter 6 Creature
    public List<GameObject> chapterSixCreatures = new List<GameObject>();
    public GameObject chapterSixCreature;
    public int chapterSixCreaturePopulation;

    //Chapter 7 Creature
    public List<GameObject> chapterSevenCreatures = new List<GameObject>();
    public GameObject chapterSevenCreature;
    public int chapterSevenCreaturePopulation;

    //Chapter 8 Creature
    public List<GameObject> chapterEightCreatures = new List<GameObject>();
    public GameObject chapterEightCreature;
    public int chapterEightCreaturePopulation;

    //Terrain
    public perlinTerrain terrain;
    public float terrainMin;

    // Start is called before the first frame update
    void Start()
    {

        //Chapter One Creature Spawning
        for(int i = 0; i < chapterOneCreaturePopulation; i++)
        {
            chapterOneCreature = Instantiate(chapterOneCreature, new Vector3(Random.Range(terrainMin, terrain.cols), Random.Range(4f, 20f), Random.Range(terrainMin, terrain.rows)), Quaternion.identity);
            chapterOneCreatures.Add(chapterOneCreature);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
