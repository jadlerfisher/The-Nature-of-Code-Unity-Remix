using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using UnityEngine.SceneManagement;

public class cellC71 : MonoBehaviour
{

    // we need a list for the cells and one for the rules
    public List<int> cells = new List<int>();
    //test set
    public List<int[]> rulesetArray = new List<int[]>();

    public int[] ruleSet0 = { 0, 1, 0, 1, 1, 0, 1, 0 };
    public int[] ruleSet1 = { 1, 0, 1, 0, 1, 0, 1, 0 };
    public int[] ruleSet2 = { 0, 1, 0, 1, 1, 0, 1, 0 };
    public int[] ruleSet3 = { 1, 1, 0, 0, 1, 0, 1, 1 };
    public int[] ruleSet4 = { 0, 0, 0, 1, 1, 0, 1, 0 };
    public int[] ruleSet5 = { 1, 0, 0, 1, 1, 0, 1, 0 };
    public int[] ruleSet6 = { 0, 1, 1, 0, 1, 0, 1, 1 };


    int generation = 0;
    int w = 10;
    public GameObject cell;
    int rulesChosen;

    // Start is called before the first frame update

    void Start()
    {
        cells.Capacity = 60;

        rulesetArray.Add(ruleSet0);
        rulesetArray.Add(ruleSet1);
        rulesetArray.Add(ruleSet2);
        rulesetArray.Add(ruleSet3);
        rulesetArray.Add(ruleSet4);
        rulesetArray.Add(ruleSet5);
        rulesetArray.Add(ruleSet6);

        // All cells start with 0 excpet the center cell has a state of 1
        for (int i = 0; i < 60; i++)
        {
            cells.Add(0);
        }
        cells[cells.Count / 2] = 1;

        rulesChosen = (int)Random.Range(0, 6);
    }

    void generate()
    {
        // compute the generation
        List<int> nextgen = new List<int>();

        for (int i = 1; i < cells.Count; i++)
        {
            nextgen.Add(cells[i]);
        }

        for (int i = 1; i < cells.Count-1; i++)
        {
            int left = cells[i - 1];
            int me = cells[i];
            int right = cells[i + 1];
            nextgen[i] = rules(left, me, right);
        }
        cells = nextgen;

        // Increment your generation counter
        generation++;
    }

    // look up a new state from the ruleset
    int rules(int a, int b, int c)
    {
        string s = a.ToString() + b.ToString() + c.ToString();
        int index = BitStringToInt(s);

        return rulesetArray[rulesChosen][index];
    }

    IEnumerator chooseRules()
    {
        //Print the time of when the function is first called.
        yield return new WaitForSeconds(4);
        rulesChosen = (int)Random.Range(0, 6);
        SceneManager.LoadScene("Chapter 7 Figure 1");
    }

    void Update() {


        for (int i = 0; i < cells.Count; i++)
        {
            Color cellColor = new Color();

            if (cells[i] == 1)
            {
                cellColor = Color.black;

                cell = Instantiate(cell, new Vector3(i, generation, 0), Quaternion.identity);
                //Fetch the Renderer component of the Cell
                Renderer c_Renderer = cell.GetComponent<Renderer>();
                c_Renderer.material.color = cellColor;
            }
            else
            {
                cellColor = Color.white;

                cell = Instantiate(cell, new Vector3(i, generation, 0), Quaternion.identity);
                //Fetch the Renderer component of the Cell
                Renderer c_Renderer = cell.GetComponent<Renderer>();
                c_Renderer.material.color = cellColor;
            }
        }
        //Generate new rules
        generate();
        StartCoroutine(chooseRules());
    }

    public static int BitStringToInt(string bitString)
    {
        byte[] bits = Encoding.ASCII.GetBytes(bitString);
        byte[] reversedBits = bits.Reverse().ToArray();

        var num = 0;
        for (var power = 0; power < reversedBits.Count(); power++)
        {
            var currentBit = reversedBits[power];
            if (currentBit == '1')
            {
                var currentNum = (int)Mathf.Pow(2, power);
                num += currentNum;
            }
        }
        return num;
    }

}
