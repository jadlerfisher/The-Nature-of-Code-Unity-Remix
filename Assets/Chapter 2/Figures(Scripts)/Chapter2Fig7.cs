using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter2Fig7 : MonoBehaviour
{
    public List<GameObject> Movers = new List<GameObject>();
    public GameObject Mover;
    public int amountMovers;

    public GameObject a;
    Vector3 force;

    private attractorChapter2_6 aC26;
    private moverChapter2_6 mC26;


    // Start is called before the first frame update
    void Start()
    {


        for (int i = 0; i < amountMovers; i++)
        {
            Mover = Instantiate(Mover);
            Mover.transform.position = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
            float scale = Random.Range(1f, 10f);
            Mover.transform.localScale = new Vector3(scale, scale, scale);
            Movers.Add(Mover);

        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {




        foreach (GameObject m in Movers)
        {


            Vector3 difference = this.transform.position - m.transform.position;
            float dist = difference.magnitude;
            Vector3 gravityDirection = difference.normalized;
            float gravity = 6.7f * (this.transform.localScale.x * m.transform.localScale.x * 80) / (dist * dist);

            Vector3 gravityVector = (gravityDirection * gravity);
            m.transform.GetComponent<Rigidbody>().AddForce(m.transform.forward*10f, ForceMode.Acceleration);
            m.transform.GetComponent<Rigidbody>().AddForce(gravityVector, ForceMode.Acceleration);
        }
    }
}
