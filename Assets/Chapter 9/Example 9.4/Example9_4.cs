using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example9_4 : MonoBehaviour
{
    // Retrieve the faces from the scene.
    [SerializeField] Face9_4[] population;
    [SerializeField] Button evolveButton;
    [SerializeField] Text generationText;
    // Example parameters:
    public float mutationRate = 0.1f;

    // Local variables.
    private int currentGeneration = 0;

    private void Start()
    {
        // Bind the UI button:
        evolveButton.onClick.AddListener(AdvanceGeneration);
    }

    // Runs when the Evolve button is pressed:
    private void AdvanceGeneration()
    {
        // Update the text.
        currentGeneration++;
        generationText.text = $"Generation {currentGeneration}";

        List<DNA9_4> genePool = Selection();
        foreach(Face9_4 face in population)
        {
            face.DNA = Reproduction(genePool);
            face.Draw();
            face.fitness = 1;
            face.fitnessText.text = "1";
        }
    }

    private List<DNA9_4> Selection()
    {
        // Create the gene pool, giving more pool entries
        // to the faces that scored a higher fitness.
        List<DNA9_4> pool = new List<DNA9_4>();
        foreach(Face9_4 face in population)
        {
            int prominence = (int)face.fitness;
            for(int i = 0; i < prominence; i++)
            {
                pool.Add(face.DNA);
            }
        }
        return pool;
    }
    private DNA9_4 Reproduction(List<DNA9_4> pool)
    {
        // Choose a random mother and father to reproduce.
        DNA9_4 partnerA = pool[Random.Range(0, pool.Count)];
        DNA9_4 partnerB = pool[Random.Range(0, pool.Count)];
        DNA9_4 child = partnerA.Crossover(partnerB);

        // Add some mutation to the child.
        child.Mutate(mutationRate);
        return child;
    }
}
