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

    private Transform newTransform;

    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        newTransform = transform;
        Chapter8Fig10Rule[] ruleset = new Chapter8Fig10Rule[1];
        ruleset[0] = new Chapter8Fig10Rule('F', "FF+[+F-F-F]-[-F+F+F]");
        lSys = new Chapter8Fig10LSystem("F", ruleset);
        turtle = new Chapter8Fig10Turtle(lSys.Sentence, Screen.height / 3, 25f); // TODO 25f should be Mathf.Deg2Rad
        redraw();
    }

    private void redraw()
    {
        transform.position = new Vector2(Screen.width / 2, Screen.height); // TODO these shoudl use Screenworldtopoint
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + Mathf.Rad2Deg * (-Mathf.PI / 2)));
        newTransform = turtle.Render(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (counter < 5)
            {
                transform.position = newTransform.position;
                transform.rotation = newTransform.rotation;
                lSys.Generate();
                turtle.Todo = lSys.Sentence;
                turtle.Len *= 0.5f;
                newTransform = transform;
                redraw();
                counter++;
            }
        }
    }
}

public class Chapter8Fig10Turtle
{
    public string Todo;
    public float Len;
    private float theta;

    public Chapter8Fig10Turtle(string s, float l, float t)
    {
        Todo = s;
        Len = l;
        theta = t;
    }

    public Transform Render(Transform currentTransform)
    {
        Transform nextTransform = currentTransform;
        // TODO Set material
        for (int i = 0; i < Todo.Length; i++)
        {            
            char[] todoCharArray = Todo.ToCharArray();
            char c = todoCharArray[i];
            if (c == 'F' || c == 'G')
            {
                drawLine(currentTransform.position, new Vector2(Len, 0));
                nextTransform.position += new Vector3(Len, 0);
            }
            else if (c == '+')
            {
                nextTransform.rotation = Quaternion.Euler(currentTransform.rotation.x, currentTransform.rotation.y, currentTransform.rotation.z + theta);
            }
            else if (c == '-')
            {
                nextTransform.rotation = Quaternion.Euler(currentTransform.rotation.x, currentTransform.rotation.y, currentTransform.rotation.z - theta);
            }
            else if (c == '[')
            {
                currentTransform = nextTransform;
            }
            else if (c == ']')
            {
                nextTransform = currentTransform;
            }
        }

        return nextTransform;
    }

    private void drawLine(Vector2 start, Vector2 end)
    {
        GameObject g = new GameObject();
        LineRenderer l = g.AddComponent<LineRenderer>();
        l.positionCount = 2;
        l.SetPosition(0, start);
        l.SetPosition(1, end);
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