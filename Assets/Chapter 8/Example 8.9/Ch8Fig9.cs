using System.Text; // Required to use StringBuilder
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Chapter 8 Figure 9: Simple L-System sentence generation
/// </summary>

public class Ch8Fig9 : MonoBehaviour
{
    [SerializeField]
    Text CanvasText;
    // Start with "A"
    private string current = "A";

    // Number of generations
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        CanvasText.text = "Generation " + count + ": " + current;
        print("Generation " + count + ": " + current);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // A new StringBuilder for the next generation
            System.Text.StringBuilder next = new StringBuilder();
            char[] currentCharString = current.ToCharArray();

            // Look through the current string to replace according
            // to L-System rules
            foreach (char c in currentCharString)
            {
                if (c == 'A')
                {
                    // If we find A replace with AB
                    next.Append("AB");
                }
                else if (c == 'B')
                {
                    // If we find B replace with A
                    next.Append("A");
                }
            }

            // The current String is now the next one
            current = next.ToString();
            count++;

            // Print to console
            CanvasText.text = "Generation " + count + ": " + current;
            print("Generation " + count + ": " + current);
        }
    }
}
