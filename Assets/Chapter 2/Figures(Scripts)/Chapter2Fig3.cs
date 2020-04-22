using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig3 : MonoBehaviour
{
    private List<Mover2_3> Movers = new List<Mover2_3>();
    // Define constant forces in our environment
    private Vector3 wind = new Vector3(0.004f, 0f, 0f);
    private Vector3 gravity = Vector3.down * 9;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 moverSpawnPosition = new Vector3(-7, 3, 0);
        // Create copys of our mover and add them to our list
        while(Movers.Count < 30)
        {
            Movers.Add(new Mover2_3(moverSpawnPosition));
        }
    }

        //Now let us alter the mass of each based on its location
        foreach (GameObject mover in Movers)
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
            gravity = new Vector3(0, .0001f * Movers[i].GetComponent<moverChapter2>().mass, 0f);

            Movers[i].GetComponent<moverChapter2>().applyForce(wind);
            Movers[i].GetComponent<moverChapter2>().applyForce(gravity);

        }
    }
}
