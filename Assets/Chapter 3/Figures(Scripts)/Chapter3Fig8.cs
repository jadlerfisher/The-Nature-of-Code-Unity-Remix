using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig8 : MonoBehaviour
{
    public float angle = 0f;
    public float angleVel = 0.2f;

    public GameObject waverPrefab;
    public int amountWavers;
    public List<GameObject> TheWave = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    { 
        for (int i = 0; i < amountWavers; i++)
        {
            GameObject Waver = Instantiate(waverPrefab);
            TheWave.Add(Waver);
        }

        for (int x = 0; x <= 90; x++)
        {
            float y = Mathf.Lerp(0f, 100f, Mathf.InverseLerp(-1f, 1f, Mathf.Sin(angle)));
            TheWave[x].transform.position = new Vector3(x, y, 0f);
            angle += angleVel;
        }
    }
}
