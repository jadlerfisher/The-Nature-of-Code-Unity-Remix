using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA9_4
{
    // Represents genetic traits on a numerical scale.
    public float[] genes;

    public DNA9_4()
    {
        // We need 18 genes to draw our face.
        genes = new float[18];
        for(int i = 0; i < genes.Length; i++)
        {
            // Generate completely random genes.
            genes[i] = Random.Range(0f, 1f);
        }
    }

    public void Mutate(float mutationRate)
    {
        // Give each gene a small chance to mutate into
        // a completely random new value.
        for(int i = 0; i < genes.Length; i++)
        {
            if(Random.Range(0f, 1f) < mutationRate)
            {
                genes[i] = Random.Range(0f, 1f);
            }
        }
    }

    public DNA9_4 Crossover(DNA9_4 partner)
    {
        DNA9_4 child = new DNA9_4();

        // Inherit some genes from each partner.
        int midpointIndex = Random.Range(0, genes.Length);
        for(int i = 0; i < genes.Length; i++)
        {
            if(i > midpointIndex) { child.genes[i] = genes[i]; }
            else { child.genes[i] = partner.genes[i]; }
        }

        return child;
    }
}
