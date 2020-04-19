using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig11 : MonoBehaviour
{

    public List<GameObject> Movers = new List<GameObject>();
    public GameObject Mover;
    public int amountMovers;
    littleMover LM;


    // Start is called before the first frame update
    void Start()
    {
        // We need to instantiate our Little Movers before we can put them in a List. 
        for (int i = 0; i < amountMovers; i++)
        {
            Mover = Instantiate(Mover);
            Movers.Add(Mover);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < Movers.Count; i++)
        {
            LM = Movers[i].GetComponent<littleMover>();
            Vector2 dir = LM.subtractVectors(mousePos, LM.location);
            LM.acceleration = LM.multiplyVector(dir.normalized, .5f);
        }
    }

}
