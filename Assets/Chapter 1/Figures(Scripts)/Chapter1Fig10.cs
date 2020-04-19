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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = LM.subtractVectors(mousePos, LM.location);
        LM.acceleration = LM.multiplyVector(dir.normalized, .5f);

    }

}
