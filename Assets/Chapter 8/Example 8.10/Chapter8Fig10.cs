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
        // P changing the transofmr only changes any object drawn AFTER the transformation, and resets to normal coordinates on the very next draw!
        // Modifying transform will change all objects even fi they're already drawn!

        
        counter = 0;
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Chapter8Fig10Rule[] ruleset = new Chapter8Fig10Rule[1];
        ruleset[0] = new Chapter8Fig10Rule('F', "FF+[+F-F-F]-[-F+F+F]");
        lSys = new Chapter8Fig10LSystem("F", ruleset);
        turtle = new Chapter8Fig10Turtle(lSys.Sentence, screenSize.y / 3, 25 * Mathf.Deg2Rad); 
        redraw();
    }

    private void redraw()
    {
        
        
        turtle.Render();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (counter < 5)
            {
                
                lSys.Generate();
                turtle.Todo= lSys.Sentence;
                turtle.Len *= 0.5f;
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

    public void Render()
    {        
        for (int i = 0; i < Todo.Length; i++)
        {
            char[] todoCharArray = Todo.ToCharArray();
            char c = todoCharArray[i];
            
            if (c == 'F' || c == 'G')
            {
                
            }
            else if (c == '+')
            {
                
            }
            else if (c == '-')
            {
                
            }
            else if (c == '[')
            {
                
            }
            else if (c == ']')
            {
                
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