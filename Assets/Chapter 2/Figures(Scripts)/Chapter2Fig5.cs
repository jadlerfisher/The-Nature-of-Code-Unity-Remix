using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig5 : MonoBehaviour
{
    public List<GameObject> Movers = new List<GameObject>();
    public GameObject Mover;
    public int amountMovers;

    private Vector3 wind;
    private Vector3 gravity;

    // Start is called before the first frame update
    void Start()
    {
        // We need to instantiate our Little Movers before we can put them in a List. 
        for (int i = 0; i < amountMovers; i++)
        {
            Mover = Instantiate(Mover);
            Mover.GetComponent<moverChapter2_5>().location = new Vector3(Random.Range(-10f, 10f), Random.Range(4f, 10f), 0f);
            Movers.Add(Mover);

        }

        //Now let us alter the mass of each based on its location
        foreach (GameObject mover in Movers)
        {
            mover.GetComponent<moverChapter2_5>().alterMass();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Apply the forces to each of the GameObjects
        for (int i = 0; i < Movers.Count; i++)
        {
            gravity = new Vector3(0, -.0001f * Movers[i].GetComponent<moverChapter2_5>().mass, 0f);

            Movers[i].GetComponent<moverChapter2_5>().applyForce(gravity);

            if (Movers[i].GetComponent<moverChapter2_5>().collided)
            {
               Movers[i].GetComponent<moverChapter2_5>().drag();
            }


        }
    }
}
