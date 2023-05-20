using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SkipScene : MonoBehaviour
{
    public string sceneName;

    public void GoToScene()
    {
        Score.wordScrambleTime = -1;

        SceneManager.LoadScene(Menu.Zumba);
    }
}
