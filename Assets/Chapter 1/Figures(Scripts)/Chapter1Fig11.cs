using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig11 : MonoBehaviour
{

    public List<GameObject> Movers = new List<GameObject>();
    public GameObject Mover;
    public int amountMovers;
    //Mouse coordinates
    Vector3 mousePosition;
    littleMover LM;
    Vector3 fixedMousePositionVector;


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

        fixedMousePosition();

    }

    // Since we are going to be following the mouse, we need to make the coordinates we receive from our vector are independent of our screen resolution. For example, on a 5K monitor, that mouse is moving across thousands of more pixels than a 2K monitor.

    void fixedMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        for (int i = 0; i < Movers.Count; i++)
        {
            LM = Movers[i].GetComponent<littleMover>();

            // I am multipling the X and Y vectors by 10F on my screen. You may need to do the same or change this number. And send the data to the Little Mover.
            LM.subtractVector(mousePos * 10F, LM.location);

        }
    }


}
