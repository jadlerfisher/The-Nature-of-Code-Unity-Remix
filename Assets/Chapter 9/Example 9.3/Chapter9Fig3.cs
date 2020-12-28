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

public class Chapter9Fig3 : MonoBehaviour
{
    [Header("Percentage chance of random thruster in children")]
    [Range(0f, 1f)]
    public float mutationRate;

    [Header("How many Rockets per generation")]
    public int populationSize;

    [Header("How long each generation lives, in update cycles")]
    public float lifetime;

    [Header("Text Object used to display simulation information")]
    public UnityEngine.UI.Text infoText;

    public GameObject rocketPrefab;

    private Chapter9Fig3Population population; // Population
    private int lifeCycle; // Time for cycle of generation
    private float recordTime; // Fastest time to target
    private Chapter9Fig3Obstacle target; // Target 
    private List<Chapter9Fig3Obstacle> obstacles; // A list to keep track of all the obstacles!

    private Vector2 screenSize; // Size of the screen in meters or UnityUnits

    // Start is called before the first frame update
    void Start()
    {
        // Sets main cam to orthographic, sets size to 10
        setupCamera();

        // Initialize variables
        lifeCycle = 0;
        recordTime = lifetime;

        // Top middle of the screen
        target = new Chapter9Fig3Obstacle(0, screenSize.y - 2f, 4f, 2f);        

        // Create a population with a mutation rate, and population max
        population = new Chapter9Fig3Population(rocketPrefab, screenSize, mutationRate, populationSize, lifetime, target);

        // Create the obstacle course
        obstacles = new List<Chapter9Fig3Obstacle>();
        obstacles.Add(new Chapter9Fig3Obstacle(0, 0, 14, 2));
    }    

    // Update is called once per frame
    void Update()
    {
        // If the generation hasn't ended yet
        if (lifeCycle < lifetime)
        {
            population.Live(obstacles);
            if (population.TargetReached() && lifeCycle < recordTime)
            {
                recordTime = lifeCycle;
            }
            lifeCycle++;
        }
        else // Otherwise a new generation
        {
            lifeCycle = 0;
            population.Fitness();
            population.Selection();
            population.Reproduction();
        }

        // Display some info
        // Using interpolated strings, denoted by the $ before the quotes, allow us to use var names in brackets
        // Inserting the \n breakout char creates a new line in the text field
        
        infoText.text = $"Generation #: {population.Generations}\n" +
            $"Cycles left: {lifetime - lifeCycle}\n" +
            $"Record cycles: {recordTime}";
    }   

    private void setupCamera()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
    }
}

/// <summary>
/// Pathfinding w/ Genetic Algorithms
/// A class for an obstacle, just a simple rectangle that is drawn
/// and can check if a Rocket touches it
/// Also using this class for target position
/// </summary>

public class Chapter9Fig3Obstacle
{
    public Vector2 Position { get; private set; }
    private float w, h;

    public Chapter9Fig3Obstacle(float x, float y, float _w, float _h)
    {
        Position = new Vector2(x, y);
        w = _w;
        h = _h;
        spawnObstacle();
    }

    private void spawnObstacle()
    {
        // Obstacle representation
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Quad);

        // Destroy components we do not need
        Object.Destroy(g.GetComponent<Collider>());

        // WebGL needs a new material
        Renderer r = g.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));

        // Set GameObject's position and scale to this object's
        g.transform.position = Position;
        g.transform.localScale = new Vector3(w, h);
    }

    public bool Contains(Vector2 spot)
    {
        if (spot.x < (Position.x + (w / 2)) && spot.x > (Position.x - (w / 2)) && spot.y < (Position.y + (h/2)) && spot.y > (Position.y - (h/2)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

/// <summary>
/// Pathfinding w/ Genetic Algorithms
/// A class to describe a population of "creatures"
/// </summary>

public class Chapter9Fig3Population
{
    private float mutationRate; // Mutation rate
    private Chapter9Fig3Rocket[] population; // Array to hold the current population
    private List<Chapter9Fig3Rocket> matingPool; // List which we will use for our "mating pool"
    public int Generations { get; private set; } // Number of generations  
    private Vector2 screenSize;
    private Chapter9Fig3Obstacle target;
    private GameObject rocketObject;

    // Initialize the population
    public Chapter9Fig3Population(GameObject rocketObj, Vector2 screen, float mutation, int numberOfRockets, float lifeTime, Chapter9Fig3Obstacle obTarget)
    {
        rocketObject = rocketObj;
        mutationRate = mutation;
        population = new Chapter9Fig3Rocket[numberOfRockets];
        matingPool = new List<Chapter9Fig3Rocket>();
        Generations = 0;
        screenSize = screen;
        target = obTarget;

        // Making a new set of creatures
        for (int i = 0; i < population.Length; i++)
        {
            Vector2 position = new Vector2(0, -screenSize.y);
            population[i] = new Chapter9Fig3Rocket(rocketObj, position, new Chapter9Fig3DNA(lifeTime), target, population.Length);
        }
    }

    public void Live(List<Chapter9Fig3Obstacle> os)
    {
        // For every creature
        for (int i = 0; i < population.Length; i++)
        {
            // If it finishes, mark it as done!
            population[i].CheckTarget();
            population[i].Run(os);
        }
    }

    // Did anything finish?
    public bool TargetReached()
    {
        for (int i = 0; i < population.Length; i++)
        {
            if (population[i].HitTarget)
                return true;
        }

        return false;
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
        float maxfitness = getMaxFitness();

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
            Chapter9Fig3Rocket mom = matingPool[m];
            Chapter9Fig3Rocket dad = matingPool[d];

            // Get their genes
            Chapter9Fig3DNA momGenes = mom.DNA;
            Chapter9Fig3DNA dadGenes = dad.DNA;

            // Mate their genes
            Chapter9Fig3DNA child = momGenes.Crossover(dadGenes);

            // Mutate their genes
            child.Mutate(mutationRate);

            // Fill the new population with the new child
            Vector2 position = new Vector2(0, -screenSize.y);
            population[i] = new Chapter9Fig3Rocket(rocketObject, position, child, target, population.Length);
        }

        Generations++;
    }

    // Find highest fitness for the population
    private float getMaxFitness()
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

public class Chapter9Fig3Rocket
{
    // Rocket representation
    private GameObject g;

    // All of our physics stuff
    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;

    // How close did it get to the target
    private float recordDist;

    // Fitness and DNA
    public float Fitness { get; private set; }
    public Chapter9Fig3DNA DNA { get; private set; }

    // To count which force we're on in the genes
    private int geneCounter = 0;

    public bool HitObstacle { get; private set; }

    // Did I reach the target?
    public bool HitTarget { get; private set; }

    // What was my finish time?
    private int finishTime;

    private Chapter9Fig3Obstacle target;

    // Constructor
    public Chapter9Fig3Rocket(GameObject rocketObj, Vector2 l, Chapter9Fig3DNA _dna, Chapter9Fig3Obstacle ob, int totalRockets)
    {
        HitTarget = false;
        target = ob;
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
        position = l;
        DNA = _dna;
        finishTime = 0;             // We're going to count how long it takes to reach target
        recordDist = 10000;         // Some high number that will be beat instantly
        HitObstacle = false;
        g = GameObject.Instantiate(rocketObj, position, Quaternion.identity);        
    }

    // Fitness function
    // distance = distance from target
    // finish = what order did I finish (first, second, etc...)
    // f(distance, finish) =   (1.0f / finish^1.5) * (1.0f / distance^6);
    // a lower finish is rewarded (exponentially) and/or shorter distance to target (exponetially)
    public void CalculateFitness()
    {
        if (recordDist < 1)
            recordDist = 1;

        // Reward for finishing faster and getting close
        Fitness = (1 / (finishTime * recordDist));

        // Make the method exponential
        Fitness = Mathf.Pow(Fitness, 4);

        if (HitObstacle) 
            Fitness *= 0.1f; // Lose 90% of fitness for hitting an obstacle

        if (HitTarget)
            Fitness *= 2; // Twice the fitness for finishing!
    }

    // Run in relation to all the obstacles
    // If I'm stuck, don't bother updating or checking for intersection

    public void Run(List<Chapter9Fig3Obstacle> os)
    {
        if (!HitObstacle && !HitTarget)
        {
            applyForce(DNA.Genes[geneCounter]);
            geneCounter = (geneCounter + 1) % DNA.Genes.Length;
            update();

            // If I hit an edge or an obstacle
            obstacles(os);
        }

        // Draw me!
        if (!HitObstacle)
        {
            display();
        }
        else
        {
            // Disable gameObject representation
            g.SetActive(false);
        }
    }

    // Did I make it to the target?
    public void CheckTarget()
    {
        float d = Vector2.Distance(position, target.Position);
        if (d < recordDist)
        {
            recordDist = d;
        }

        if (target.Contains(position) && !HitTarget) // Target is now OBSTACLE
        {
            HitTarget = true;
        }
        else if (!HitTarget)
        {
            finishTime++;
        }
    }

    // Did I hit an obstacle?
    private void obstacles(List<Chapter9Fig3Obstacle> os)
    {
        // Goes through each object in our os list. During each one, we can use obs to manipulate the one we're on.
        foreach (Chapter9Fig3Obstacle obs in os)
        {
            if (obs.Contains(position))
            {
                HitObstacle = true;
            }
        }
    }

    private void applyForce(Vector2 f)
    {
        acceleration += f;
    }

    private void update()
    {
        velocity += acceleration;
        position += velocity;
        acceleration *= 0;
    }

    private void display()
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

public class Chapter9Fig3DNA
{
    // The genetic sequence
    public Vector2[] Genes { get; private set; }

    // The maximum strength of the forces
    private float maxForce = 0.01f;

    // Constructor (makes a DNA of random Vector2s)
    public Chapter9Fig3DNA(float lifeTime)
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
    public Chapter9Fig3DNA(Vector2[] newGenes)
    {
        Genes = newGenes;
    }

    // CROSSOVER
    // Creates new DNA sequence from two (this & and a partner)
    public Chapter9Fig3DNA Crossover(Chapter9Fig3DNA partner)
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

        Chapter9Fig3DNA newGenes = new Chapter9Fig3DNA(child);
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
