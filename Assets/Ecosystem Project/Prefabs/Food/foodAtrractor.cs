using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodAtrractor : MonoBehaviour
{

    public float G = 657f;
    public float mass;
    Vector3 location;
    public GameObject predator;
    public string predatorTag = "";
    public bool alive = true;

    createAnimals animalKingdom;

    // Start is called before the first frame update
    void Start()
    {
        animalKingdom = GameObject.Find("Scripts").GetComponent<createAnimals>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
           


            GameObject[] predators = GameObject.FindGameObjectsWithTag(predatorTag);
            if (predators.Length > 0)
            {
                foreach (GameObject predator in predators)
                {

                    predator.transform.GetComponent<Rigidbody>().AddForce(predator.transform.forward, ForceMode.Acceleration);
                    predator.transform.GetComponent<Rigidbody>().AddForce(attract(predator), ForceMode.Acceleration);
                   
                    location = this.gameObject.transform.position;

                    float dist = Vector3.Distance(predator.transform.position, location);

                    if (dist <= 4f)
                    {
                        alive = false;
                        if (this.gameObject.name == "redbird(Clone)" || this.gameObject.name == "redbird")
                        {

                            animalKingdom.redBirdList.Remove(this.gameObject);

                        }
                        else if (this.gameObject.name == "berry(Clone)" || this.gameObject.name == "berry")
                        {
                            animalKingdom.berryList.Remove(this.gameObject);
                        }
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
            }
        }

    }

    public Vector3 attract(GameObject predator)
    {
        Vector3 difference = location - predator.transform.position;
        float dist = difference.magnitude;
        Vector3 gravityDirection = difference.normalized;
        float gravity = 6.7f * (mass * predator.GetComponent<Rigidbody>().mass) / (dist * dist);

        Vector3 gravityVector = (gravityDirection * gravity);

        return gravityVector;
    }
}
