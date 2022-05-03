using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Face9_4 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Get components from the scene:
    [SerializeField] RectTransform faceEdge, mouth, leftEye, rightEye;
    [SerializeField] public Text fitnessText;

    [System.NonSerialized] public float fitness = 1;
    [System.NonSerialized] public DNA9_4 DNA;
    private Image faceEdgeImage, mouthImage, leftEyeImage, rightEyeImage;

    private void Start()
    {
        // Gather the image components of each transform.
        // We will use these images to change the fill color,
        // and we will use the transforms to change the sizing/layout.
        faceEdgeImage = faceEdge.GetComponent<Image>();
        mouthImage = mouth.GetComponent<Image>();
        leftEyeImage = leftEye.GetComponent<Image>();
        rightEyeImage = rightEye.GetComponent<Image>();

        // Generate a new random DNA for this face.
        DNA = new DNA9_4();
        Draw();
    }

    // Update the UI elements to reflect the DNA.
    public void Draw()
    {
        // Set colors for each image component:
        faceEdgeImage.color = new Color(DNA.genes[0], DNA.genes[1], DNA.genes[2]);
        mouthImage.color = new Color(DNA.genes[3], DNA.genes[4], DNA.genes[5]);
        Color eyeColor = new Color(DNA.genes[6], DNA.genes[7], DNA.genes[8]);
        leftEyeImage.color = rightEyeImage.color = eyeColor;

        // Set the outer face size.
        faceEdge.localScale = Mathf.Lerp(0.2f, 1, DNA.genes[9]) * Vector2.one;

        // Get four values for the corners of the mouth.
        float mouthLeft = DNA.genes[10];
        float mouthRight = Mathf.Lerp(mouthLeft, 1, DNA.genes[11]);
        float mouthBottom = DNA.genes[12];
        float mouthTop = Mathf.Lerp(mouthBottom, 1, DNA.genes[13]);

        // Restrict the mouth to the lower half of the face.
        mouthBottom *= 0.5f;
        mouthTop *= 0.5f;

        // Apply the corners of the mouth to the UI:
        mouth.anchorMin = new Vector2(mouthLeft, mouthBottom);
        mouth.anchorMax = new Vector2(mouthRight, mouthTop);

        // Get four values for the corner of the left eye.
        float eyeLeft = DNA.genes[14];
        float eyeRight = Mathf.Lerp(eyeLeft, 1, DNA.genes[15]);
        float eyeBottom = DNA.genes[16];
        float eyeTop = Mathf.Lerp(eyeBottom, 1, DNA.genes[17]);

        // Restrict the eye to the upper left quadrant of the face.
        eyeBottom = Mathf.Lerp(0.5f, 1, eyeBottom);
        eyeTop = Mathf.Lerp(0.5f, 1, eyeTop);
        eyeLeft *= 0.5f;
        eyeRight *= 0.5f;

        // Apply the corners of the eye to the left eye:
        leftEye.anchorMin = new Vector2(eyeLeft, eyeBottom);
        leftEye.anchorMax = new Vector2(eyeRight, eyeTop);

        // Apply the corners of the eye to the right eye(mirrored about the y axis):
        rightEye.anchorMin = new Vector2(1 - eyeRight, eyeBottom);
        rightEye.anchorMax = new Vector2(1 - eyeLeft, eyeTop);
    }

    // Define the behavior to increase the fitness value every frame:
    private IEnumerator IncreaseFitness()
    {
        while(true)
        {
            fitness += Time.deltaTime * 10;
            fitnessText.text = ((int)fitness).ToString();
            yield return null;
        }
    }

    // Enabled and disable the fitness increaser.
    private IEnumerator increaser;

    public void OnPointerEnter(PointerEventData eventData)
    {
        increaser = IncreaseFitness();
        StartCoroutine(increaser);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(increaser);
    }
}
