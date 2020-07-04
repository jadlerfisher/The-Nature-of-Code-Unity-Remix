using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chapter5Fig3 : MonoBehaviour
{
    ObjectWall wall;
    Cannons cannon;
    // Start is called before the first frame update
    void Start()
    {
        wall = new ObjectWall();
        cannon = new Cannons();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("r"))
        {
            cannon.cannonFire("ray");
        }
        else if (Input.GetKey("b"))
        {
            cannon.cannonFire("box");
        }
        else if (Input.GetKey("c"))
        {
            cannon.cannonFire("capsule");
        }
        else if (Input.GetKey("s"))
        {
            cannon.cannonFire("sphere");
        }
    }
}

    public class Cannons
    {
        float m_MaxDistance = 100f;
        bool m_HitDetect;
        float m_Force = -200f;
        float m_ExplosiveRadius;

        Collider m_Collider;

         RaycastHit m_Hit;
         Ray ray;

        public Cannons()
        {

        }

        public void cannonFire(string rayChoice)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (rayChoice == "ray")
            {
                if (Physics.Raycast(ray, out m_Hit, 100))
                {
                    Rigidbody hitBody = m_Hit.transform.gameObject.GetComponent<Rigidbody>();
                    m_ExplosiveRadius = 100f;
                    hitBody.AddExplosionForce(m_Force, m_Hit.transform.position, m_ExplosiveRadius);
                }
            }
            else if (rayChoice == "box")
            {
            GameObject boxObject = projectile("b");
            m_HitDetect = Physics.BoxCast(ray.origin, boxObject.transform.localScale, ray.direction, out m_Hit, boxObject.transform.rotation, m_MaxDistance);
            if (m_HitDetect)
                {
                    Rigidbody hitBody = m_Hit.transform.gameObject.GetComponent<Rigidbody>();
                    m_ExplosiveRadius = 100f;
                    hitBody.AddExplosionForce(m_Force, m_Hit.transform.position, m_ExplosiveRadius);
                }
            }
            else if (rayChoice == "capsule")
            {
                projectile("c");

                Vector3 p1 = ray.origin + m_Collider.bounds.center + Vector3.up * -m_Collider.bounds.extents.y * 0.5F;
                Vector3 p2 = p1 + Vector3.up * m_Collider.bounds.extents.y;

            m_HitDetect = Physics.CapsuleCast(p1, p2, m_Collider.bounds.extents.x, ray.direction, out m_Hit, m_MaxDistance);
                if (m_HitDetect)
                {
                    Rigidbody hitBody = m_Hit.transform.gameObject.GetComponent<Rigidbody>();
                    m_ExplosiveRadius = 100f;
                    hitBody.AddExplosionForce(m_Force, m_Hit.transform.position, m_ExplosiveRadius);
                }
            }
            else if (rayChoice == "sphere")
            {
                GameObject sphereObject = projectile("s");

            m_HitDetect = Physics.SphereCast(ray, sphereObject.transform.localScale.x, out m_Hit, m_MaxDistance); 
            if (m_HitDetect)
                {
                    Rigidbody hitBody = m_Hit.transform.gameObject.GetComponent<Rigidbody>();
                    m_ExplosiveRadius = 100f;
                    hitBody.AddExplosionForce(m_Force, m_Hit.transform.position, m_ExplosiveRadius);
                }
            }
        }

        GameObject projectile(string projectileCode)
        {
            if (projectileCode == "r")
            {
                return null;
            }
            else if (projectileCode == "b")
            {
                GameObject boxObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

                Renderer renderer = boxObject.GetComponent<Renderer>();
                renderer.enabled = false;

                boxObject.transform.localScale = new Vector3(3f, 3f, 3f);
                return boxObject;
            }
            else if (projectileCode == "c")
            {
                GameObject capsuleObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);

                Renderer renderer = capsuleObject.GetComponent<Renderer>();
                renderer.enabled = false;

                capsuleObject.transform.localScale = new Vector3(6f, 3f, 3f);
                m_Collider = capsuleObject.GetComponent<Collider>();
               
                return capsuleObject;
            }
            else if (projectileCode == "s")
            {
                GameObject sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                Renderer renderer = sphereObject.GetComponent<Renderer>();
                renderer.enabled = false;

                sphereObject.transform.localScale = new Vector3(3f, 3f, 3f);
                m_Collider = sphereObject.GetComponent<Collider>();

                return sphereObject;
            }
            else
            {
                return null;
            }
        }
    }

public class ObjectWall
{
    // How many columns and how many rows in the grid?
    private int columns, rows;

    // Resolution of grid relative to window width and height in pixels
    private int resolution;

    public ObjectWall()
    {
        resolution = 50;
        columns = Screen.width / resolution; // Total columns equals width divided by resolution
        rows = Screen.height / resolution; // Total rows equals height divided by resolution
        initializeObjectWall();
    }

    void initializeObjectWall()
    {
        float xOff = 0;
        for (int i = -20; i < columns; i++) // Using a nested loop to hit every column and every row of the flow field
        {
            float yOff = 0;
            for (int j = -20; j < rows; j++)
            {
                GameObject wallObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                wallObject.transform.position = new Vector3(i, j, -5f);

                Renderer renderer = wallObject.GetComponent<Renderer>();
                renderer.material = new Material(Shader.Find("Diffuse"));

                Rigidbody body = wallObject.AddComponent<Rigidbody>();
                body.useGravity = false;

                yOff += 0.1f;
            }
            xOff += 0.1f;
        }
    }
}