using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Example 6.10 L-System
/// </summary>

public struct SavedTransform
{
    public Vector3 Position;
    public Quaternion Rotation;
}

public class Chapter8Fig10 : MonoBehaviour
{
    private Chapter8Fig10LSystem lSys;
    private Vector2 screenSize;
    private string todo;
    private float len;
    private float theta;
    private Stack<SavedTransform> savedTransforms;
    private SavedTransform currentTransform;

    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        savedTransforms = new Stack<SavedTransform>();
        currentTransform.Position = transform.position;
        currentTransform.Rotation = transform.rotation;
        counter = 0;
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        
        Chapter8Fig10Rule[] ruleset = new Chapter8Fig10Rule[1];
        ruleset[0] = new Chapter8Fig10Rule('F', "FF+[+F-F-F]-[-F+F+F]");
        lSys = new Chapter8Fig10LSystem("F", ruleset);

        todo = lSys.Sentence;
        len = screenSize.y / 3;
        theta = Mathf.Rad2Deg * (-Mathf.PI / 2);

        redraw();
    }

    private void saveTransform()
    {
        currentTransform.Position = transform.position;
        currentTransform.Rotation = transform.rotation;
    }

    private void pushTransformStack()
    {
        savedTransforms.Push(currentTransform);
    }

    private SavedTransform popTransformStack()
    {
        return savedTransforms.Pop();
    }

    private void redraw()
    {
        transform.position = new Vector2(0f, -screenSize.y + len);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + Mathf.Rad2Deg * (-Mathf.PI / 2));
        saveTransform();
        render();
    }

    private void render()
    {
        for (int i = 0; i < todo.Length; i++)
        {
            char[] todoCharArray = todo.ToCharArray();
            char c = todoCharArray[i];
            if (c == 'F' || c == 'G')
            {
                GameObject g = new GameObject();
                g.transform.position = currentTransform.Position;
                g.transform.rotation = currentTransform.Rotation;
                LineRenderer line = g.AddComponent<LineRenderer>();
                line.positionCount = 2;
                line.useWorldSpace = true;
                line.SetPosition(0, Vector2.zero);
                line.SetPosition(1, new Vector2(len, 0));
                currentTransform.Position += new Vector3(len, 0);
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;
                
            }
            else if (c == '+')
            {
                currentTransform.Rotation = Quaternion.Euler(currentTransform.Rotation.x, currentTransform.Rotation.y, currentTransform.Rotation.z + theta);
                
            }
            else if (c == '-')
            {
                currentTransform.Rotation = Quaternion.Euler(currentTransform.Rotation.x, currentTransform.Rotation.y, currentTransform.Rotation.z - theta);
                
            }
            else if (c == '[')
            {
                pushTransformStack();
            }
            else if (c == ']')
            {
                currentTransform = popTransformStack();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (counter < 5)
            {
                pushTransformStack();
                lSys.Generate();
                todo = lSys.Sentence;
                len *= 0.5f;
                currentTransform = popTransformStack();
                redraw();
                counter++;
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