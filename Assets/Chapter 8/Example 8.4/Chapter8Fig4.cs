using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter8Fig4 : MonoBehaviour
{
    private Vector2 minPos, maxPos;


    void Start()
    {
        findWindowLimits();
        cantor(minPos.x + 3, maxPos.y - 2, (maxPos.x - minPos.x) - 6);
    }

    void cantor(float x, float y, float len)
    {
        line(x, y, x + len, y); // Create the first line

        if (len >= 0.01f) // Stop at 0.01 meters length!
        {
            y -= 2;

            cantor(x, y, len / 3); // Call the function recursively 
            cantor(x + len * 2 / 3, y, len / 3); // We need two lines for each line above it
        }
    }

    private GameObject line(float x1, float y1, float x2, float y2) 
    {
        GameObject obj = new GameObject(); // Creates an object to attach a Line Renderer to
        obj.name = "Line"; // We change the in-scene name so it's easier to identify
        LineRenderer line = obj.AddComponent<LineRenderer>(); // We attach the LineRenderer to the object and store it as 'line'

        line.material = new Material(Shader.Find("Sprites/Default")); // Set the material
        line.startColor = line.endColor = Color.black; // Set the color

        line.startWidth = line.endWidth = 1f; // Set the width

        line.SetPosition(0, new Vector3(x1, y1, 0)); // Set the begining and end points of the line
        line.SetPosition(1, new Vector3(x2, y2, 0));

        return obj;
    }

    private void findWindowLimits()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
        minPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        maxPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
