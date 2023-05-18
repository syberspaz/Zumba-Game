using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deactivate : MonoBehaviour
{
    public string targetSceneName; // The name of the scene where the object should be active

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != targetSceneName)
        {
            gameObject.SetActive(false); // Deactivate the game object if the current scene is not the target scene
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}

