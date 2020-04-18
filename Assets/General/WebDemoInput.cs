using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class WebDemoInput : MonoBehaviour
{
    // Demonstration states:
    private bool isUnloaded = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isUnloaded = !isUnloaded;
            // Destroy all other gameobjects in the scene except for this one.
            if (isUnloaded)
            {
                foreach (GameObject obj in FindObjectsOfType<GameObject>())
                    if (obj != this.gameObject)
                        Destroy(obj);
            }
            // Reload the scene.
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
