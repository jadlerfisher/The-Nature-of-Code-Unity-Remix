using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Example 7.3 Game of Life OOP
/// </summary>

public class Chapter7Figure3 : MonoBehaviour
{
    // Each cell is now an object!

    private Chapter7Figure3GOL gol;

    // Start is called before the first frame update
    void Start()
    {
        gol = new Chapter7Figure3GOL();
        setOrthographicCamera();
        limitFrameRate();
    }

    // Update is called once per frame
    void Update()
    {
        // Reset board when mouse is pressed
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        gol.Generate();
        gol.Display();
    }

    private void setOrthographicCamera()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
    }

    private void limitFrameRate()
    {        
        QualitySettings.vSyncCount = 0;
    }
}

public class Chapter7Figure3Cell
{
    // GameObject properties, to visually represent and draw our cell
    private GameObject cellRep;
    private Material cellMaterial;

    // Size of the screen in meters, or Unity units
    private Vector2 screenSize;
    private float yScreenOffset; // Cells are spawned with a small offset so they spawn in more centered    
    private float xScreenOffset;

    // Location
    private float x, y;    

    public int State;
    public int Previous;

    public Chapter7Figure3Cell(int _x, int _y)
    {
        x = _x;
        y = _y;

        // How big our screen is in World Units
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        xScreenOffset = 9f;
        yScreenOffset = 3f;

        State = Random.Range(0, 2);
        Previous = State;
        createGameObject();
    }   

    public void SavePrevious()
    {
        Previous = State;
    }

    public void NewState(int newState)
    {
        State = newState;
    }

    public void Display()
    {
        if (Previous == 0 && State == 1) // On reproduction
        {
            cellMaterial.color = Color.blue;
        }
        else if (State == 1) // On continuing to stay alive
        {
            cellMaterial.color = Color.black;
        }
        else if (Previous == 1 && State == 0) // On death
        {
            cellMaterial.color = Color.red;
        }
        else // On continuing to stay dead
        {
            cellMaterial.color = Color.white;
        }

        cellRep.transform.position = new Vector3((x * cellRep.transform.localScale.x) - screenSize.x - xScreenOffset, 
                                                 (y * cellRep.transform.localScale.x) - screenSize.y - yScreenOffset);
    }

    private void createGameObject()
    {
        cellRep = GameObject.CreatePrimitive(PrimitiveType.Quad);

        // Scale is halved so the entire board is displayed in screen
        cellRep.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Renderer r = cellRep.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        cellMaterial = r.material;
        Object.Destroy(cellRep.GetComponent<Collider>());
    }
}

public class Chapter7Figure3GOL
{
    // Initialize rows, columns and set-up array
    private int columns, rows;    

    private Chapter7Figure3Cell[,] board;

    public Chapter7Figure3GOL()
    {        
        columns = 72;
        rows = 41;
        board = new Chapter7Figure3Cell[columns, rows];
        innit();
    }

    private void innit()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i, j] = new Chapter7Figure3Cell(i, j);
            }
        }
    }

    // The process of creating the new generation
    public void Generate()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i, j].SavePrevious();
            }
        }

        // Loop through every spot in our 2D array and check spots neighbors
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {

                // Add up all the states in a 3x3 surrounding grid
                int neighbors = 0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        neighbors += board[(x + i + columns) % columns, (y + j + rows) % rows].Previous;
                    }
                }

                // A little trick to subtract the current cell's state since
                // we added it in the above loop
                neighbors -= board[x,y].Previous;

                // Rules of Life
                if ((board[x,y].State == 1) && (neighbors < 2)) board[x,y].NewState(0);           // Loneliness
                else if ((board[x,y].State == 1) && (neighbors > 3)) board[x,y].NewState(0);           // Overpopulation
                else if ((board[x,y].State == 0) && (neighbors == 3)) board[x,y].NewState(1);           // Reproduction
                                                                                                          // else do nothing!
            }
        }
    }

    public void Display() // Draw the cells. Since the cells are now objects, they are responsible for their own drawing
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i,j].Display();
            }
        }
    }
}
