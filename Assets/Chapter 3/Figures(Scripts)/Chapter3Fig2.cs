using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig2 : MonoBehaviour
{
    public List<GameObject> Movers = new List<GameObject>();
    public GameObject Mover;
    public int amountMovers;

    Vector3 force;

    public GameObject attractor;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountMovers; i++)
        {
            Mover = Instantiate(Mover);
            Mover.transform.position = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
            float scale = Random.Range(.1f, 2f);
            Mover.transform.localScale = new Vector3(scale, scale, scale);
            Movers.Add(Mover);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject mover in Movers)
        {
            mover.GetComponent<moverChapter3_2>().attract(attractor);
        }
    }
}
