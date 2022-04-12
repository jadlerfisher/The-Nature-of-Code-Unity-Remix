using UnityEngine;

/// <summary>
/// Example 7.2 Game of Life
/// </summary>

public class Chapter7Figure2 : MonoBehaviour
{
    private Chapter7Fig2GameOfLife gol;

    // Start is called before the first frame update
    void Start()
    {
        gol = new Chapter7Fig2GameOfLife();
        SetOrthographicCamera();
        LimitFrameRate();
    }

    // Update is called once per frame
    void Update()
    {
        // Reset board if mouse is pressed
        if (Input.GetMouseButtonDown(0))
        {
            gol.Restart();
        }
        gol.Generate();
        gol.Display();        
    }

    private void SetOrthographicCamera()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
    }

    private void LimitFrameRate()
    {
        Application.targetFrameRate = 24;
        QualitySettings.vSyncCount = 0;
    }
}

public class Chapter7Fig2GameOfLife
{
    private int columns, rows;
    private Vector2 screenSize; 
    private float yScreenOffset; // Cells are spawned with a small offset so they spawn in more centered    

    // One array for conceptual board, another to draw and change Unity GameObjects used for displaying
    private int[,] board;
    private GameObject[,] unityBoard;

    public Chapter7Fig2GameOfLife()
    {        
        // How big our screen is in World Units
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        yScreenOffset = 2f;
        columns = 72;
        rows = 41;
        unityBoard = new GameObject[columns, rows];
        board = new int[columns, rows];
        SpawnUnityBoard();
        Innit();
    }

    private void SpawnUnityBoard() // Very inneficient to spawn new GO every frame, so we just change the color instead
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject newCell = GameObject.CreatePrimitive(PrimitiveType.Quad);

                // Scale is halved so the entire board is displayed in screen
                newCell.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                Renderer r = newCell.GetComponent<Renderer>();
                r.material = new Material(Shader.Find("Diffuse"));
                Object.Destroy(newCell.GetComponent<Collider>());                
                unityBoard[i, j] = newCell;

                // Set position based to lower left of screen, with screen offset
                // Since we shrunk the objects, we'll have to compensate when setting x and y position
                newCell.transform.position = new Vector3((i * newCell.transform.localScale.x) - screenSize.x,
                                                         (j * newCell.transform.localScale.y) - screenSize.y + yScreenOffset);
            }
        }
    }

    private void Innit()
    {
        for (int i = 1; i < columns - 1; i++)
        {
            for (int j = 1; j < rows - 1; j++)
            {
                board[i, j] = Random.Range(0, 2);
            }
        }
    }

    public void Restart()
    {
        Innit();
    }

    // The process of creating the new generation
    public void Generate()
    {
        int[,] next = new int[columns, rows];

        // Loop through every spot in our 2D array and check spots neighbors
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                // Add up all the states in a 3x3 surrounding grid
                int neighbors = 0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        neighbors += board[x + i, y + j];
                    }
                }

                // A little trick to subtract the current cell's state since
                // we added it in the above loop
                neighbors -= board[x, y];

                // Rules of Life
                if ((board[x,y] == 1) && (neighbors < 2)) next[x,y] = 0;        // Loneliness
                else if ((board[x,y] == 1) && (neighbors > 3)) next[x,y] = 0;   // Overpopulation
                else if ((board[x,y] == 0) && (neighbors == 3)) next[x,y] = 1;  // Reproduction
                else next[x,y] = board[x,y];                                    // Stasis
            }
        }

        // Next is now our board
        board = next;
    }

    // Draw the cells by changing the material's color based on its state
    public void Display()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Renderer r = unityBoard[i,j].GetComponent<Renderer>();
                if (board[i,j] == 1)
                {                    
                    r.material.color = Color.black;
                }
                else 
                {
                    r.material.color = Color.white;
                }
            }
        }
    }
}
