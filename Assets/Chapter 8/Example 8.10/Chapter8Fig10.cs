using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Example 6.10 L-System
/// </summary>

public class Chapter8Fig10 : MonoBehaviour
{
    private Chapter8Fig10LSystem lSys;
    private Chapter8Fig10Turtle turtle;

    private Vector2 screenSize;

    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject g = new GameObject();
        //LineRenderer l = g.AddComponent<LineRenderer>();
        ////l.positionCount = 2;
        //l.SetPosition(0, new Vector2(0, 0));
        //l.SetPosition(1, new Vector2(100, 0));
        //l.useWorldSpace = false; // lines origin will be GO's origin!

        counter = 0;
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Chapter8Fig10Rule[] ruleset = new Chapter8Fig10Rule[1];
        ruleset[0] = new Chapter8Fig10Rule('F', "FF+[+F-F-F]-[-F+F+F]");
        lSys = new Chapter8Fig10LSystem("F", ruleset);
        turtle = new Chapter8Fig10Turtle(lSys.Sentence, screenSize.y / 3, 25 * Mathf.Deg2Rad); // TODO Should screen.height be screenSize.y / 3 ?
        redraw();
    }

    private void redraw()
    {
        transform.position += new Vector3(screenSize.x / 2, screenSize.y);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + (Mathf.Rad2Deg * (-Mathf.PI / 2)));
        turtle.Render();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (counter < 5)
            {
                // pushMatrix
                lSys.Generate();
                Debug.Log(lSys.Sentence); // TODO remove
                turtle.Todo = lSys.Sentence;
                turtle.Len *= 0.5f;
                // popMatrix
                counter++;
                redraw();
            }
        }
    }
}

public class Chapter8Fig10Turtle
{
    public string Todo;
    public float Len;
    private float theta;
    private Transform g;

    public Chapter8Fig10Turtle(string s, float l, float t)
    {
        g = new GameObject().transform; // TODO doesn't need to be GO, use Transomf
        Todo = s;
        Len = l;
        theta = t;
    }

    public void Render()
    {
        for (int i = 0; i < Todo.Length; i++)
        {
            GameObject go = new GameObject();
            go.transform.position = g.transform.position;
            go.transform.rotation = g.transform.rotation;
            char[] todoCharArray = Todo.ToCharArray();
            char c = todoCharArray[i];
            if (c == 'F' || c == 'G')
            {
                LineRenderer l = go.AddComponent<LineRenderer>();
                //l.GetComponent<Renderer>().material = Shader.Find("Diffuse");
                l.useWorldSpace = false;
                l.SetPosition(0, new Vector2(go.transform.position.x, go.transform.position.y));
                l.SetPosition(1, new Vector2(Len, go.transform.position.y));
                go.transform.position += new Vector3(Len, 0);
            }
            else if (c == '+')
            {
                go.transform.rotation = Quaternion.Euler(new Vector3(go.transform.rotation.x, go.transform.rotation.y, go.transform.rotation.z + theta));
            }
            else if (c == '-')
            {
                go.transform.rotation = Quaternion.Euler(new Vector3(go.transform.rotation.x, go.transform.rotation.y, go.transform.rotation.z - theta));
            }
            else if (c == '[')
            {
                g.transform.position = go.transform.position;
                g.transform.rotation = go.transform.rotation;
            }
            else if (c == ']') // Not really necessary
            {
                go.transform.position = g.transform.position;
                go.transform.rotation = g.transform.rotation;
            }
        }
    }
}

public class Chapter8Fig10LSystem
{
    public string Sentence { get; private set; }
    private Chapter8Fig10Rule[] ruleset;
    public int Generation { get; private set; }

    public Chapter8Fig10LSystem(string axiom, Chapter8Fig10Rule[] r)
    {
        Sentence = axiom;
        ruleset = r;
        Generation = 0;
    }

    public void Generate()
    {
        char[] sentenceCharArray = Sentence.ToCharArray();
        System.Text.StringBuilder nextGen = new StringBuilder();
        for (int i = 0; i < Sentence.Length; i++)
        {
            char curr = sentenceCharArray[i];
            string replace = "" + curr;
            for (int j = 0; j < ruleset.Length; j++)
            {
                char a = ruleset[j].A;
                if (a == curr)
                {
                    replace = ruleset[j].B;
                    break;
                }
            }
            nextGen.Append(replace);
        }
        Sentence = nextGen.ToString();
        Generation++;
    }

}

public class Chapter8Fig10Rule
{
    public char A { get; private set; }
    public string B { get; private set; }

    public Chapter8Fig10Rule(char _a, string _b)
    {
        A = _a;
        B = _b;
    }
}