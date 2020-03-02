using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig3 : MonoBehaviour
{
    public GameObject Mover;
    //Mouse coordinates
    Vector3 mousePosition;
    moverChapter3_3 LM;
    Vector3 fixedMousePositionVector;

    // Start is called before the first frame update
    void Start()
    {
        Mover = Instantiate(Mover);
        LM = Mover.GetComponent<moverChapter3_3>();
    }

    // Update is called once per frame
    void Update()
    {
        fixedMousePosition();
    }

    // Since we are going to be following the mouse, we need to make the coordinates we receive from our vector are independent of our screen resolution. For example, on a 5K monitor, that mouse is moving across thousands of more pixels than a 2K monitor.

    void fixedMousePosition()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(Mover.transform.position);
        Vector3 dir = Input.mousePosition;
        Vector3 needlessDir = dir - pos;
        //Because the mouse position is in 2D spac 
        LM.angleFloat = Mathf.Atan2(needlessDir.y, needlessDir.x) * Mathf.Rad2Deg;

        Vector3 dirMove = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        LM.subtractVector(dirMove*10, LM.location);

    }
}
