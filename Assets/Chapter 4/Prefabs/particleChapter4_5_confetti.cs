using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleChapter4_5_confetti : particleChapter4_Base
{
    public Quaternion perlinRotation = new Quaternion();

    public particleChapter4_5_confetti()
    {
        perlinRotation.eulerAngles = new Vector3(Mathf.Cos(1), Mathf.Sin(1), 0);

    }

   public particleChapter4_5_confetti(Vector3 location) : base (location)
    {
        float theta = ExtensionMethods.Remap(Mathf.PerlinNoise(location.x, location.y), 0f, 1f, 0f, 6.2831855f);

        perlinRotation.eulerAngles = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);

    }

   public void rotate() {
        float theta = ExtensionMethods.Remap(0f, 360f, 1f, 0f, 6.2831855f);
        Vector3 newRotation = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
        Quaternion perlinRotation = new Quaternion();
        perlinRotation.eulerAngles = newRotation*100;

        this.gameObject.transform.rotation = perlinRotation;
    }

}
