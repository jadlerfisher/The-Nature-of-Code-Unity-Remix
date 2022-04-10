using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Chapter 7 Figure 1: Wolfram Elementary Cellular Automata
/// </summary>

public class Chapter7Fig1 : MonoBehaviour 
{
    // A list to store ruleset arrays
    [SerializeField] List<int[]> rulesetList = new List<int[]>();

    // Custom Rulesets
    [SerializeField] int[] ruleSet0 = { 0, 1, 0, 1, 1, 0, 1, 0 };
    [SerializeField] int[] ruleSet1 = { 1, 0, 1, 0, 1, 0, 1, 0 };
    [SerializeField] int[] ruleSet2 = { 0, 1, 0, 1, 1, 0, 1, 0 };
    [SerializeField] int[] ruleSet3 = { 1, 1, 0, 0, 1, 0, 1, 1 };
    [SerializeField] int[] ruleSet4 = { 0, 0, 0, 1, 1, 0, 1, 0 };
    [SerializeField] int[] ruleSet5 = { 1, 0, 0, 1, 1, 0, 1, 0 };
    [SerializeField] int[] ruleSet6 = { 0, 1, 1, 0, 1, 0, 1, 1 };
    
    private int rulesChosen;

    // An object to describe a Wolfram elementary Cellular Automata
    Chapter7Fig1CA ca;

    // How long after the CA has been drawn before reloading the scene, choosing new rule
    private int delay = 0;

    // Start is called before the first frame update
    void Start()
    {
        AddRuleSetsToList();

        // Choosing a random rule set using Random.Range
        rulesChosen = Random.Range(0, rulesetList.Count);
        int[] ruleset = rulesetList[rulesChosen]; 
        ca = new Chapter7Fig1CA(ruleset); // Initialize CA

        LimitFrameRate();
        SetOrthographicCamera();
    }    

    private void Update() 
    {
        ca.Display(); // Draw the CA
        ca.Generate();

        if (ca.IsFinished()) // If we're done, clear the screen, pick a new ruleset and restart
        {
            delay++;
            if (delay > 30) 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene                
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene            
        }           
    }

    private void AddRuleSetsToList()
    {
        rulesetList.Add(ruleSet0);
        rulesetList.Add(ruleSet1);
        rulesetList.Add(ruleSet2);
        rulesetList.Add(ruleSet3);
        rulesetList.Add(ruleSet4);
        rulesetList.Add(ruleSet5);
        rulesetList.Add(ruleSet6);
    }

    private void SetOrthographicCamera()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
    }

    private void LimitFrameRate()
    {
        Application.targetFrameRate = 30; 
        QualitySettings.vSyncCount = 0;
    }
}

public class Chapter7Fig1CA
{
    private int[] cells; // An array of 0s and 1s
    private int generation; // How many generations?
    private int[] ruleset; // An array to store the ruleset, for example {0,1,1,0,1,1,0,1}
    private int rowWidth; // How wide to make the array
    private int cellCapacity; // We limit how many cells we instantiate
    private int numberOfCells; // Which needs us to keep count
    private Vector2 screenSize; 
    private float yScreenOffset; // Cells are spawned with a small offset so they spawn in centered
    private float xScreenOffset; 

    public Chapter7Fig1CA(int[] ruleSetToUse)
    {
        rowWidth = 17;
        cellCapacity = 700;
        yScreenOffset = 0.5f;
        xScreenOffset = -0.5f;

        // How big our screen is in World Units
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));        
        numberOfCells = 0;
        ruleset = ruleSetToUse;
        cells = new int[cellCapacity / rowWidth];
        Restart();
    }

    public void Randomize() // If we wanted to make a random Ruleset
    {
        for (int i = 0; i < 8; i++)
        {
            ruleset[i] = Random.Range(0, 2);
        }
    }

    private void Restart()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = 0;
        }
        cells[cells.Length / 2] = 1; // We arbitrarily start with just the middle cell having a state of "1"
        generation = 0;
    }

    // The process of creating the new generation
    public void Generate()
    {
        // First we create an empty array for the new values
        int[] nextGen = new int[cells.Length];

        // For every spot, determine new state by examing current state, and neighbor states
        // Ignore edges that only have one neighor
        for (int i = 1; i < cells.Length-1; i++)
        {
            int left = cells[i - 1]; // Left neighbor state
            int me = cells[i]; // Current state
            int right = cells[i + 1]; // Right neighbor state
            nextGen[i] = Rules(left, me, right); // Compute next generation state based on ruleset
        }

        // The current generation is the new generation
        cells = nextGen;
        generation++;
    }
    
    public void Display() // Drawing the cells. Cells with a state of 1 are black, cells with a state of 0 are white
    {
     if (numberOfCells <= cellCapacity)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                GameObject newCell = GameObject.CreatePrimitive(PrimitiveType.Quad);                
                numberOfCells++;                
                Renderer r = newCell.GetComponent<Renderer>();
                r.material = new Material(Shader.Find("Diffuse"));
                Object.Destroy(newCell.GetComponent<BoxCollider>());
                if (cells[i] == 1)
                {
                    r.material.color = Color.black;
                }
                else
                {
                    r.material.color = Color.white;
                }

                // Set position based to lower left of screen, with screen offset
                newCell.transform.position = new Vector3(i - screenSize.x - xScreenOffset, 
                                                generation - screenSize.y + yScreenOffset);
            }
        }
    }

    private int Rules (int a, int b, int c) // Implementing the Wolfram rules
    {
        if (a == 1 && b == 1 && c == 1) return ruleset[0];
        if (a == 1 && b == 1 && c == 0) return ruleset[1];
        if (a == 1 && b == 0 && c == 1) return ruleset[2];
        if (a == 1 && b == 0 && c == 0) return ruleset[3];
        if (a == 0 && b == 1 && c == 1) return ruleset[4];
        if (a == 0 && b == 1 && c == 0) return ruleset[5];
        if (a == 0 && b == 0 && c == 1) return ruleset[6];
        if (a == 0 && b == 0 && c == 0) return ruleset[7];
        return 0;
    }

    public bool IsFinished() // The CA is done if it reaches a height limit
    {
        if (generation > Screen.height / rowWidth)
            return true;
        else
            return false;
    }
}
