using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig10 : MonoBehaviour
{

    public GameObject Mover;
    //Mouse coordinates
    Vector3 mousePosition;
    littleMover LM;
    Vector3 fixedMousePositionVector;

    // Start is called before the first frame update
    void Start()
    {
        Mover = Instantiate(Mover);
        LM = Mover.GetComponent<littleMover>();
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
        LM.subtractVector(mousePos*10, LM.location);
    }
}
