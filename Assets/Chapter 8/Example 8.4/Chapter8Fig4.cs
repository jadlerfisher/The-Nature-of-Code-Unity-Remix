using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter8Fig4 : MonoBehaviour
{
    private Vector2 maximumPos;

    void Start()
    {
        FindWindowLimits();
        Cantor(-maximumPos.x + 3, maximumPos.y - 2, (maximumPos.x - -maximumPos.x) - 6);
    }

    void Cantor(float x, float y, float len)
    {
        // Create the first line
        Line(x, y, x + len, y);

        // Stop at 0.01 meters length!
        if (len >= 0.01f) 
        {
            y -= 2;

            // Call the function recursively 
            Cantor(x, y, len / 3);
            // We need two lines for each line above it
            Cantor(x + len * 2 / 3, y, len / 3); 
        }
    }

    private GameObject Line(float x1, float y1, float x2, float y2) 
    {
        // Creates an object to attach a Line Renderer to
        GameObject obj = new GameObject();
        // We change the in-scene name so it's easier to identify
        obj.name = "Line";
        // We attach the LineRenderer to the object and store it as 'line'
        LineRenderer line = obj.AddComponent<LineRenderer>();

        // Set the material
        line.material = new Material(Shader.Find("Sprites/Default"));
        // Set the color
        line.startColor = line.endColor = Color.black;
        // Set the width
        line.startWidth = line.endWidth = 1f;

        // Set the begining and end points of the line
        line.SetPosition(0, new Vector3(x1, y1, 0)); 
        line.SetPosition(1, new Vector3(x2, y2, 0));

        return obj;
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
