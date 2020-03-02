using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig2 : MonoBehaviour
{
    public List<GameObject> Movers = new List<GameObject>();
    public GameObject Mover;
    public int amountMovers;

    private Vector3 wind;
    private Vector3 gravity;

    // Start is called before the first frame update
    void Start()
    {
        gravity = new Vector3(0f, 0.0001f, 0f);
        wind = new Vector3(0.00001f, 0f, 0f);

        // We need to instantiate our Little Movers before we can put them in a List. 
        for (int i = 0; i < amountMovers; i++)
        {
            Mover = Instantiate(Mover);
            Mover.GetComponent<moverChapter2>().location = new Vector3 (Random.Range(-5f,5f), Random.Range(0f,1f), 0f);
            Movers.Add(Mover);  

        }

        //Now let us alter the mass of each based on its location
        foreach(GameObject mover in Movers)
        {
            mover.GetComponent<moverChapter2>().alterMass();
        }

    }

    // Update is called once per frame
    void Update()
    {

        //Apply the forces to each of the GameObjects
        for (int i = 0; i < Movers.Count; i++)
        {
            Movers[i].GetComponent<moverChapter2>().applyForce(wind);
            Movers[i].GetComponent<moverChapter2>().applyForce(gravity);
        }
    }
}
