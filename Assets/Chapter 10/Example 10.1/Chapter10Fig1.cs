using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter10Fig1 : MonoBehaviour
{
    //The Perceptron
    [SerializeField]
    Perceptron ptron;

    //2,000 training points
    List<Trainer> trainingPoints = new List<Trainer>();
    int count = 0;
    public int trainerCount;

    //Lines
    GameObject theLine;
    LineRenderer lR;

    // Variables to limit the mover within the screen space
    private Vector2 maximumPos;

    //Forumla for the line
    float f(float x)
    {
        return 2 * x + 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Next we grab the maximum values of the screen
        FindWindowLimits();

        //Draw the initial Line Position
        theLine = new GameObject();
        lR = theLine.AddComponent<LineRenderer>();
        lR.material = new Material(Shader.Find("Diffuse"));
        lR.material.color = Color.black;
        lR.SetPosition(0, new Vector2(-maximumPos.x, -maximumPos.y));
        lR.SetPosition(1, new Vector2(maximumPos.x, maximumPos.y));

        //Create the Perceptron based on 3 weights points of perception
        ptron = new Perceptron(3);

        //Make training points
        for (int i = 0; i < trainerCount; i++)
        {
            float x = Random.Range(-maximumPos.x, maximumPos.x);
            float y = Random.Range(-maximumPos.y, maximumPos.y);

            //Is the correct answer 1 or -1?
            int answer = 1;
            if (y < f(x))
            {
                answer = -1;
            }

            trainingPoints.Add(new Trainer(x, y, answer));
        }
    }

    // Update is called once per frame
    void Update()
    {
        ptron.Train(trainingPoints[count].inputs, trainingPoints[count].answer);
        count = (count + 1) % trainingPoints.Count;

        for (int i = 0; i < count; i++)
        {
            int guess = ptron.Feedforward(trainingPoints[i].inputs);
            //If the guess is greater than 0, paint the sphere black. Otherwise white.
            if (guess > 0)
            {
                GameObject fill = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                fill.transform.position = new Vector2(trainingPoints[i].inputs[0], trainingPoints[i].inputs[1]);
                Renderer r = fill.GetComponent<Renderer>();
                r.material = new Material(Shader.Find("Diffuse"));
                r.material.color = Color.black;

            }
            else
            {
                GameObject noFill = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                noFill.transform.position = new Vector2(trainingPoints[i].inputs[0], trainingPoints[i].inputs[1]);
                Renderer r = noFill.GetComponent<Renderer>();
                r.material = new Material(Shader.Find("Diffuse"));
                r.material.color = Color.white;
            }

            // Draw the line based on the current weights
            // Formula is weights[0]*x + weights[1]*y + weights[2] = 0
            List<float> weights = ptron.GetWeights();
            lR.SetPosition(0, new Vector2(-maximumPos.x, (-weights[2] - weights[0] * -maximumPos.x) / weights[1]));
            lR.SetPosition(1, new Vector2(maximumPos.x, (-weights[2] - weights[0] * maximumPos.x) / weights[1]));

        }
    }

    private void FindWindowLimits()
    {
        // We want to start by setting the camera's projection to Orthographic mode
        Camera.main.orthographic = true;

        // For FindWindowLimits() to function correctly, the camera must be set to coordinates 0, 0, -10
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Next we grab the maximum position for the screen
        maximumPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}


public class Perceptron
{
    //The Perceptron stores its weightsand Learning Constants
    List<float> weights = new List<float>();
    float c = 0.1f;

    public Perceptron(int n)
    {
        for(int i= 0; i < n; i++)
        {
            //Weights start off random
            weights.Add(Random.Range(-1f, 1f));
        }
    }

    //Return an output based on inputs
    public int Feedforward(List<float> inputs)
    {
        float sum = 0f;

        for(int i = 0; i < weights.Count; i++)
        {
            sum += inputs[i] * weights[i];
        }

        return Activate(sum);
    }

    //Output is a +1 or -1
    int Activate(float sum)
    {
        if (sum > 0) return 1;
        else return -1;
    }

    //Train the network against known data
    public void Train(List<float> inputs, int desired)
    {
        int guess = Feedforward(inputs);
        float error = desired - guess;
        for (int i = 0; i < weights.Count; i++)
        {
            weights[i] += c * error * inputs[i];
        }
    }

   public List<float> GetWeights()
   {
        return weights;
   }
}

public class Trainer
{
    //A "Trainer" object stores the inputs and the correct answer
    public List<float> inputs = new List<float>();
    public int answer;

    public Trainer(float x, float y, int a)
    {
        inputs.Add(x);
        inputs.Add(y);
        //Note that the Trainer has the bias built into its array
        inputs.Add(1);

        answer = a;
    }

}
