using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipScene : MonoBehaviour
{
    public string sceneName;

    public void GoToScene()
    {
        Score.wordScrambleTime = -1;

        SceneManager.LoadScene(sceneName);
    }
}
