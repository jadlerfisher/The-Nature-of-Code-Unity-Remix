using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter7Fig1 : MonoBehaviour
{
    Chapter7Fig1CA ca;

    // Start is called before the first frame update
    void Start()
    {
        int[] ruleset = { 0, 1, 0, 1, 1, 0, 1, 0 };
        ca = new Chapter7Fig1CA(ruleset);        
    }

    private void Update()
    {
        ca.Display();
        ca.Generate();
    }
}

public class Chapter7Fig1CA
{
    private int[] cells;
    private int generation;
    private int[] ruleset; // TODO make these exposed
    private int w = 5;

    public Chapter7Fig1CA(int[] r)
    {
        ruleset = r;
        cells = new int[Screen.width / w];
        restart();
    }

    public void Randomize()
    {
        for (int i = 0; i < 8; i++)
        {
            ruleset[i] = Random.Range(0, 2);
        }
    }

    private void restart()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = 0;
        }
        cells[cells.Length / 2] = 1;
        generation = 0;
    }

    public void Generate()
    {
        int[] nextGen = new int[cells.Length];
        for (int i = 1; i < cells.Length-1; i++)
        {
            int left = cells[i - 1];
            int me = cells[i];
            int right = cells[i + 1];
            nextGen[i] = rules(left, me, right);
        }

        cells = nextGen;
        generation++;
    }

    public void Display()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            GameObject newCell = GameObject.CreatePrimitive(PrimitiveType.Cube); // TODO Plane? Quad?
            Renderer r = newCell.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Diffuse"));
            Object.Destroy(newCell.GetComponent<BoxCollider>());
            if (cells[i] == 1)
            {
                r.material.color = Color.black;
            }
            else
            {
                r.material.color = Color.white;
            }
            newCell.transform.position = new Vector3(i * w, generation);
        }
    }

    private int rules (int a, int b, int c)
    {
        if (a == 1 && b == 1 && c == 1) return ruleset[0];
        if (a == 1 && b == 1 && c == 0) return ruleset[1];
        if (a == 1 && b == 0 && c == 1) return ruleset[2];
        if (a == 1 && b == 0 && c == 0) return ruleset[3];
        if (a == 0 && b == 1 && c == 1) return ruleset[4];
        if (a == 0 && b == 1 && c == 0) return ruleset[5];
        if (a == 0 && b == 0 && c == 1) return ruleset[6];
        if (a == 0 && b == 0 && c == 0) return ruleset[7];
        return 0;
    }

    public bool IsFinished()
    {
        if (generation > Screen.height / w)
            return true;
        else
            return false;
    }
}
