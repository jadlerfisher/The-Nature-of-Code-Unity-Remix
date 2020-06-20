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
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        // In P, changing the coordinate matrix only affects objects that are drawn after the manipulation, not before. So we can't child objects to this GO and change the transform after the fact as the child objects will match the new Transform.

        // In P, any manipulation done to the coordinate matrices get reset at the start of every draw.

        

        
        //transform.position = new Vector3(1f, 0f, 0f);
        //transform.rotation = Quaternion.Euler(0f, 1f, 0f);
        //Debug.Log(transform.position);
        //Debug.Log(transform.rotation);
        
        counter = 0;
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(10f, 10f));
        
        Chapter8Fig10Rule[] ruleset = new Chapter8Fig10Rule[1];
        ruleset[0] = new Chapter8Fig10Rule('F', "FF+[+F-F-F]-[-F+F+F]");
        lSys = new Chapter8Fig10LSystem("F", ruleset);
        
        
        turtle = new Chapter8Fig10Turtle(lSys.Sentence, screenSize.y / 3, 25); 
        redraw();
    }

    private void redraw()
    {
        Vector3 newPos;
        Quaternion newRot;
        transform.rotation = Quaternion.Euler(Vector3.zero); // We can't parent new objects to this object
        transform.position = new Vector3(0f, screenSize.y); // Bottom center of screen
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90);
        transform.RotateAround(new Vector3(-screenSize.x, -screenSize.y), Vector3.right, -90f); // Unity rotates objects around the object's center while P rotates the entire coordinates
        turtle.Render(out newPos, out newRot); // Render probably needs to know what the current transform pos and rot are
        transform.position = newPos;
        transform.rotation = newRot;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (counter < 5)
            {
                // pushMatrix   
                previousPosition = transform.position;
                previousRotation = transform.rotation;
                //Debug.Log(transform.position); 
                lSys.Generate();
                turtle.Todo= lSys.Sentence;
                turtle.Len *= 0.5f;
                // popMatrix
                transform.position = previousPosition;
                transform.rotation = previousRotation;
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
    private Vector3 currentPosition;
    private Vector3 savedPosition;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Quaternion currentRotation;
    private Quaternion savedRotation;
    private Transform currentTransform;

    // Transforms are reference-based so if we make a new var and assign it to the GO's Transform, if we manipulate the new var's values, the GO's transform will change along with it. So we use Vector's and Quaternions as they are structs 

    public Chapter8Fig10Turtle(string s, float l, float t)
    {
        Todo = s;
        Len = l;
        theta = t;
        
    }

    public void Render(out Vector3 newPos, out Quaternion newQuat)
    {
        currentPosition = originalPosition;
        currentRotation = originalRotation;
        for (int i = 0; i < Todo.Length; i++)
        {
            char[] todoCharArray = Todo.ToCharArray();
            char c = todoCharArray[i];
            
            if (c == 'F' || c == 'G')
            {
                GameObject g = new GameObject();
                g.transform.position = currentPosition;
                g.transform.rotation = currentRotation;
                LineRenderer r = g.AddComponent<LineRenderer>();
                r.useWorldSpace = false;
                r.positionCount = 2;
                r.SetPosition(0, g.transform.position);
                r.SetPosition(1, new Vector3(g.transform.position.x + Len, g.transform.position.y));
                currentPosition += new Vector3(Len, 0);
            }
            else if (c == '+')
            {
                currentRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z + theta);
            }
            else if (c == '-')
            {
                currentRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z - theta);
            }
            else if (c == '[')
            {
                savedPosition = currentPosition;
                savedRotation = currentRotation;
            }
            else if (c == ']')
            {
                currentPosition = savedPosition;
                currentRotation = savedRotation;
            }
        }
        newPos = currentPosition;
        newQuat = currentRotation;
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