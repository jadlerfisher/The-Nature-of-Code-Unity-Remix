using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Fig8 : MonoBehaviour
{
    // Create your speed variables for the mover class
    public GameObject Mover;
    // Start is called before the first frame update
    void Start()
    {
        Mover = Instantiate(Mover);
    }


}
