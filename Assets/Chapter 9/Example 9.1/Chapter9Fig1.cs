using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chapter9Fig1 : MonoBehaviour
{
    [SerializeField] float mutationRate;
    int totalPopulation = 150;

    DNA[] population;
    List<DNA> matingPool;
    [SerializeField] string target;
    [SerializeField] int totalIterations;
    [SerializeField] float timeBetweenGenerations;
    private int currentIteration = 0;

    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private TextMeshProUGUI display1;

    // Start is called before the first frame update
    void Start()
    {
        target = "to be or not to be";
        mutationRate = 0.01f;

        InitializePopulation();   //STEP 1

        StartCoroutine(nameof(NewGeneration));
    }

    void InitializePopulation()
    {
        population = new DNA[totalPopulation];

        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new DNA(target);
        }
    }

    void Selection()
    {
        for (int i = 0; i < population.Length; i++)
        {
            population[i].CalculateFitness();
        }

        matingPool = new List<DNA>();

        for (int i = 0; i < population.Length; i++)
        {
            int n = (int)(population[i].fitness * 100);

            for (int j = 0; j < n; j++)
            {
                matingPool.Add(population[i]);
            }
        }
    }

    void Reproduction()
    {

        for (int i = 0; i < population.Length; i++)
        {
            int a = (int)Random.Range(0, matingPool.Count);
            int b = (int)Random.Range(0, matingPool.Count);
            DNA partnerA = matingPool[a];
            DNA partnerB = matingPool[b];

            DNA child = partnerA.Crossover(partnerB);

            child.Mutate(mutationRate);

            population[i] = child;
        }
    }

    private IEnumerator NewGeneration()
    {
        Selection(); //STEP 2
        Reproduction(); //STEP 3

        string s = "";
        for (int i = 0; i < population.Length; i++)
        {
            s += population[i].getPhrase() + "     ";
        }
        display.text = s;
        display1.text = "Generation: " + currentIteration;

        if (currentIteration < totalIterations)
        {
            currentIteration++;
            yield return new WaitForSeconds(timeBetweenGenerations);
            StartCoroutine(nameof(NewGeneration));
        }
    }
}

class DNA
{
    char[] genes;
    public float fitness;
    string target;

    public DNA(string t)
    {
        target = t;
        genes = new char[target.Length];
        for (int i = 0; i < genes.Length; i++)
        {
            genes[i] = (char)Random.Range(32, 128);
        }
    }

    public void CalculateFitness()
    {
        int score = 0;
        for (int i = 0; i < genes.Length; i++)
        {
            if (genes[i] == target.ToCharArray(0, target.Length)[i])
            {
                score++;
            }
        }
        fitness = (float)score / target.Length;
    }

    public DNA Crossover(DNA partner)
    {
        DNA child = new DNA(target);
        int midpoint = (int)Random.Range(0, genes.Length);

        for (int i = 0; i < genes.Length; i++)
        {
            if (i > midpoint)
            {
                child.genes[i] = genes[i];
            }
            else
            {
                child.genes[i] = partner.genes[i];
            }
        }

        return child;
    }

    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < genes.Length; i++)
        {
            if (Random.value < mutationRate)
            {
                genes[i] = (char)Random.Range(32, 128);
            }
        }
    }

    public string getPhrase()
    {
        return new string(genes);
    }
}