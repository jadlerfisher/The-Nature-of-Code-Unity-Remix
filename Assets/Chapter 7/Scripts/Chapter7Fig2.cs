using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter7Fig2 : MonoBehaviour
{
    public Dictionary<Vector2, int> board = new Dictionary<Vector2, int>();
    public Dictionary<Vector2, int> next = new Dictionary<Vector2, int>();
    public Dictionary<Vector2, GameObject> cells = new Dictionary<Vector2, GameObject>();

    //How many columns and how many rows in the grid?
    int cols, rows;
    //Resolution of grid relative to window width and height in pixels
    int resolution;
    int neighbors;

    public GameObject cell;

    // Start is called before the first frame update
    void Start()
    {
        resolution = 100;
        cols = Camera.main.pixelWidth / resolution;
        rows = Camera.main.pixelHeight / resolution;

        float xoff = 0;
        for (int i = 0; i < cols; i++)
        {
            float yoff = 0;
            for (int j = 0; j < rows; j++)
            {
                int cellType = (int)Random.Range(0, 2);
                board.Add(new Vector2(i, j), cellType);
                cell = Instantiate(cell, new Vector3(i, j, 0), Quaternion.identity);
                cells.Add(new Vector2(i, j), cell);
            }
        }

        //foreach (KeyValuePair<Vector2, int> kvp in board)
        //{
        //    //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        //    Debug.Log("Key = " + kvp.Key + "Value = {1}" + kvp.Value);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 1; x < cols - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        Vector2 neighborLocation = new Vector2(x + i, y + j);
                        if (board.ContainsKey(neighborLocation))
                        {
                            neighbors += board[neighborLocation];
                        }
                    }
                }

                Vector2 personalLocation = new Vector2(x, y);
                neighbors -= board[personalLocation];


                // The rules of life!
                if ((board[personalLocation] == 1) && (neighbors < 2))
                {
                    next[personalLocation] = 0;

                }
                else if ((board[personalLocation] == 1) && (neighbors > 3))
                {
                    next[personalLocation] = 0;

                }
                else if ((board[personalLocation] == 0) && (neighbors == 3))
                {
                    next[personalLocation] = 1;

                }
                else
                {
                    next[personalLocation] = board[personalLocation];
                }
            }
        }
        board = next;
        StartCoroutine(live());
    }

    IEnumerator live()
    {
        //Print the time of when the function is first called.
        yield return new WaitForSeconds(2);
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector2 boardPosition = new Vector2(i, j);

                //Debug.Log(boardPosition);

                Debug.Log(board[boardPosition] + " this is it");

                if (board.TryGetValue(boardPosition, out int test)) ;
                {

                   if (test == 1)
                    {
                    if (cells.TryGetValue(boardPosition, out GameObject cell))
                    {
                        Color cellColor = Color.black;

                        Renderer c_Renderer = cell.GetComponent<Renderer>();
                        c_Renderer.material.color = cellColor;
                    }
                    }
                    else
                    {
                        if (cells.TryGetValue(boardPosition, out GameObject cell))
                        {
                            Color cellColor = Color.white;

                            Renderer c_Renderer = cell.GetComponent<Renderer>();
                            c_Renderer.material.color = cellColor;
                        }
                    }
                    }
                }
            }
        }
    }

