using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example 9.2 Smart Rockets w/ Genetic Algorithms
/// Each Rocket's DNA is an array of Vector2's
/// Each Vector2 acts as a force for each frame of animation
/// Imagine a booster on the end of the rocket that can point in any direction
/// And fire at any strength every fram
/// 
/// The Rocket's fitness is a function of how close it gets to the target as well as how fast it gets there
/// 
/// This example is inspired by Jer Thorp's Smart Rockets
/// http://www.blprnt.com/smartrockets/
/// </summary>

public class Chapter9Fig2 : MonoBehaviour
{
    [Header("Percentage chance of random thruster in children")]
    [Range(0f, 1f)]
    [SerializeField] float mutationRate;

    [Header("How many Rockets per generation")]
    [SerializeField] int populationSize;

    [Header("How long each generation lives, in update cycles")]
    [SerializeField] float lifetime;

    [Header("Text Object used to display simulation information")]
    [SerializeField] UnityEngine.UI.Text infoText;

    [Header("Rocket prefab to instantiate")]
    [SerializeField] GameObject rocketPrefab;

    private Chapter9Fig2Population population; // Population
    private int lifeCounter; // Timer for cycle of generation
    private Vector2 targetPosition; // Target position
    private Vector2 maximumPos; // Size of the screen in meters or UnityUnits

    // Start is called before the first frame update
    void Start()
    {
        // Sets main cam to orthographic, sets size to 10
        FindWindowLimits();

        // Initialize variables
        lifeCounter = 0;

        // Top middle of the screen
        targetPosition = new Vector2(0, maximumPos.y - 2f);

        // Create a population with a mutation rate, and population max
        population = new Chapter9Fig2Population(rocketPrefab, maximumPos, mutationRate, populationSize, lifetime, targetPosition);
        
        drawTargetPosition();
    }    

    // Update is called once per frame
    void Update()
    {
        // If the generation hasn't ended yet
        if (lifeCounter < lifetime)
        {
            population.Live();
            lifeCounter++;
        }
        else // Otherwise a new generation
        {
            lifeCounter = 0;
            population.Fitness();
            population.Selection();
            population.Reproduction();
        }

        // Display some info
        // Using interpolated strings, denoted by the $ before the quotes, allow us to use var names in brackets
        // Inserting the \n breakout char creates a new line in the text field
        
        infoText.text = $"Generation #: {population.Generations}\nCycles left: {lifetime - lifeCounter}";
    }   

    private void drawTargetPosition()
    {
        GameObject target = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.Destroy(target.GetComponent<Collider>());
        Renderer r = target.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        target.transform.position = targetPosition;
    }
    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // Set the desired camera size
        Camera.main.orthographicSize = 10;

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}

/// <summary>
/// Pathfinding w/ Genetic Algorithms
/// A class to describe a population of "creatures"
/// </summary>

public class Chapter9Fig2Population
{
    private float mutationRate; // Mutation rate
    private Chapter9Fig2Rocket[] population; // Array to hold the current population
    private List<Chapter9Fig2Rocket> matingPool; // List which we will use for our "mating pool"
    public int Generations { get; private set; } // Number of generations  
    private Vector2 screenSize;
    private Vector2 targetPosition;
    private GameObject rocketObject;

    // Initialize the population
    public Chapter9Fig2Population(GameObject rocketObj, Vector2 screen, float mutation, int numberOfRockets, float lifeTime, Vector2 targetPos)
    {
        rocketObject = rocketObj;
        mutationRate = mutation;
        screenSize = screen;
        targetPosition = targetPos;
        population = new Chapter9Fig2Rocket[numberOfRockets];
        matingPool = new List<Chapter9Fig2Rocket>();
        Generations = 0;
        

        // Making a new set of creatures
        for (int i = 0; i < population.Length; i++)
        {
            Vector2 position = new Vector2(0, -screenSize.y);
            population[i] = new Chapter9Fig2Rocket(rocketObj, position, new Chapter9Fig2DNA(lifeTime), targetPos);
        }
    }

    public void Live()
    {
        // Run every rocket
        for (int i = 0; i < population.Length; i++)
        {
            population[i].Run();
        }
    }

    // Calculate fitness for each creature
    public void Fitness()
    {
        for (int i = 0; i < population.Length; i++)
        {
            population[i].CalculateFitness();
        }
    }

    // Generate a mating pool
    public void Selection()
    {
        // Clear the list
        matingPool.Clear();        

        // Calculate total fitness of whole population
        float maxfitness = GetMaxFitness();

        // Calculate fitness for each member of the population (scaled to value between 0 and 1)
        // Based on fitness, each member will get added to the mating pool a certain number of times
        // A higher fitness = more entries to mating pool = more likely to be picked as a parent
        // A lower fitness = fewer entries to mating pool = less likely to be picked as a parent
        for (int i = 0; i < population.Length; i++)
        {
            float fitnessNormal = 0 + (population[i].Fitness - 0) * (1 - 0) / (maxfitness - 0);
            int n = (int)fitnessNormal * 100; // Arbitary multiplier
            for (int j = 0; j < n; j++)
            {
                matingPool.Add(population[i]);
            }
        }
    }

    // Making the next generation
    public void Reproduction()
    {
        // Refill the population with children from the mating pool
        for (int i = 0; i < population.Length; i++)
        {
            // Destroy all rockets in population
            population[i].Death();

            // Spin the wheel of fourtune to pick two new parents
            int m = Random.Range(0, matingPool.Count);
            int d = Random.Range(0, matingPool.Count);

            // Pick two parents
            Chapter9Fig2Rocket mom = matingPool[m];
            Chapter9Fig2Rocket dad = matingPool[d];

            // Get their genes
            Chapter9Fig2DNA momGenes = mom.DNA;
            Chapter9Fig2DNA dadGenes = dad.DNA;

            // Mate their genes
            Chapter9Fig2DNA child = momGenes.Crossover(dadGenes);

            // Mutate their genes
            child.Mutate(mutationRate);

            // Fill the new population with the new child
            Vector2 position = new Vector2(0, -screenSize.y);
            population[i] = new Chapter9Fig2Rocket(rocketObject, position, child, targetPosition);
        }

        Generations++;
    }

    // Find highest fitness for the population
    private float GetMaxFitness()
    {
        float record = 0f;
        for (int i = 0; i < population.Length; i++)
        {
            if (population[i].Fitness > record)
            {
                record = population[i].Fitness;
            }
        }

        return record;
    }
}

/// <summary>
/// Pathfinding w/ Genetic Algorithms
/// Rocket class -- this is just like our Boid / Particle class
/// the only difference is that it has DNA & fitness
/// </summary>

public class Chapter9Fig2Rocket
{
    // Rocket representation
    private GameObject g;

    // All of our physics stuff
    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;

    // Fitness and DNA
    public float Fitness { get; private set; }
    public Chapter9Fig2DNA DNA { get; private set; }

    // To count which force we're on in the genes
    private int geneCounter = 0;

    // Did I reach the target?
    private bool hitTarget = false; 

    private Vector2 targetPosition;

    // Constructor
    public Chapter9Fig2Rocket(GameObject rocketObj, Vector2 l, Chapter9Fig2DNA _dna, Vector2 targetPos)
    {
        targetPosition = targetPos;
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
        position = l;
        DNA = _dna;
        g = Object.Instantiate(rocketObj, position, Quaternion.identity);
    }    

    // Fitness function
    // fitness = one divided by distance squared
    public void CalculateFitness()
    {
        float d = Vector2.Distance(position, targetPosition);
        Fitness = Mathf.Pow(1 / d, 2);
    }

    // Run in relation to all the obstacles
    // If I'm stuck, don't bother updating or checking for intersection
    public void Run()
    {
        CheckTarget(); // Check to see if we've reached the target
        if (!hitTarget)
        {
            ApplyForce(DNA.Genes[geneCounter]);
            geneCounter = (geneCounter + 1) % DNA.Genes.Length;
            UpdateMovement();
        }

        Display();
    }


    // Did I make it to the target?
    private void CheckTarget()
    {
        float d = Vector2.Distance(position, targetPosition);
        if (d < 1)
        {
            hitTarget = true;
        }
    }

    private void ApplyForce(Vector2 f)
    {
        acceleration += f;
    }

    private void UpdateMovement()
    {
        velocity += acceleration;
        position += velocity;
        acceleration *= 0;
    }

    private void Display()
    {
        // Gives us our angle of rotation based on our current velocity
        float theta = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        g.transform.position = position;

        // Transform.rotation are Quaternions. Quaternion.Euler will allow us to use degrees and convert it to Quaternion
        // Adjusted z rotation on cone by -90 degrees
        g.transform.rotation = Quaternion.Euler(g.transform.rotation.x , g.transform.rotation.y , g.transform.rotation.z + theta - 90);
    }

    public void Death()
    {
        Object.Destroy(this.g.gameObject);
    }
}

/// <summary>
/// DNA is an array of vectors
/// </summary>

public class Chapter9Fig2DNA
{
    // The genetic sequence
    public Vector2[] Genes { get; private set; }

    // The maximum strength of the forces
    private float maxForce = 0.01f;

    // Constructor (makes a DNA of random Vector2s)
    public Chapter9Fig2DNA(float lifeTime)
    {
        Genes = new Vector2[(int)lifeTime];
        for (int i = 0; i < Genes.Length; i++)
        {
            // Random angle
            float angle = Random.Range(0f, 360f);
            Genes[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Genes[i] *= Random.Range(0f, maxForce);
        }
    }

    // Constructor #2, creates the instance based on an existing array
    public Chapter9Fig2DNA(Vector2[] newGenes)
    {
        Genes = newGenes;
    }

    // CROSSOVER
    // Creates new DNA sequence from two (this & and a partner)
    public Chapter9Fig2DNA Crossover(Chapter9Fig2DNA partner)
    {
        Vector2[] child = new Vector2[Genes.Length];
        // Pick a midpoint
        int crossover = Random.Range(0, Genes.Length);
        // Take "half from one and "half" from the other
        for (int i = 0; i < Genes.Length; i++)
        {
            if (i > crossover)
            {
                child[i] = Genes[i];
            }
            else
            {
                child[i] = partner.Genes[i];
            }
        }

        Chapter9Fig2DNA newGenes = new Chapter9Fig2DNA(child);
        return newGenes;
    }

    // Based on a mutation probability, picks a new random Vector
    public void Mutate(float m)
    {
        for (int i = 0; i < Genes.Length; i++)
        {
            if (Random.Range(0f, 1f) < m)
            {
                float angle = Random.Range(0f, 360f);
                Genes[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Genes[i] *= Random.Range(0f, maxForce);
            }
        }
    }
}
