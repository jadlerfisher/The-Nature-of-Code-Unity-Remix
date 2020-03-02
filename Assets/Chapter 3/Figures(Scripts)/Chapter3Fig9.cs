using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter3Fig9 : MonoBehaviour
{

    float angle = 0f;
    float angleVel = 0.4f;
    float amplitude = 100f;

    public GameObject waver;
    public int amountWavers;
    Vector3 p;
    public List<GameObject> TheWave = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        p = Camera.main.ViewportToWorldPoint(new Vector3(100, 100, Camera.main.nearClipPlane));

        for (int i = 0; i < amountWavers; i++)
        {
            GameObject Waver = Instantiate(waver);
            Waver.transform.position = new Vector3(Random.Range(-100f, 100f), 0f, 0f);
            TheWave.Add(Waver);
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int x = 0; x <= 90; x++)
        {
            angle += angleVel;

            float y = map(Mathf.Sin(angle), -1f, 1f, 0f, 10f);
            TheWave[x].transform.position = new Vector3(x, y, 0f);

        }

    }



    public float map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}