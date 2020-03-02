using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowFieldChapter6_4 : MonoBehaviour
{
    public Dictionary<Vector2, Vector3> fieldArray = new Dictionary<Vector2, Vector3>();
    public List<Vector3> fieldRotations = new List<Vector3>();
    public GameObject flowArrow;
    //How many columns and how many rows in the grid?
    int cols, rows;
    //Resolution of grid relative to window width and height in pixels
    int resolution;

    // Start is called before the first frame update
    void Start()
    {
        resolution = 10;
        cols = Camera.main.pixelWidth / resolution;
        rows = Camera.main.pixelHeight / resolution;
        
        float xoff = 0;
        for (int i = 0; i < cols; i++)
        {
            float yoff = 0;
            for (int j = 0; j < rows; j++)
            {
                //The mathematical concept of Tau ( or 2*PI) is 6.2831855(ish)
                float theta = ExtensionMethods.Remap(Mathf.PerlinNoise(xoff, yoff), 0f, 1f, 0f, 6.2831855f);
                
                Quaternion perlinRotation = new Quaternion();
                Vector3 perlinVectors = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
                perlinRotation.eulerAngles = perlinVectors*100f;
                fieldRotations.Add(perlinRotation.eulerAngles);

                fieldArray.Add(new Vector2(i, j), perlinVectors);

                Instantiate(flowArrow, new Vector3(i,j,0), perlinRotation);
                yoff += 0.1f;
            }
            xoff += 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 lookup(Vector2 lookup)
    {
        int column = (int)lookup.x;
        int row = (int)lookup.y;
        Vector2 gridLocation = new Vector2(column, row);
        if (fieldArray.ContainsKey(gridLocation))
        {
            return new Vector3(fieldArray[gridLocation].x, fieldArray[gridLocation].y, fieldArray[gridLocation].z);
        }
        else
        {
            Debug.Log("out of field");
            return new Vector3(0,0,0);

        }
    }

}
