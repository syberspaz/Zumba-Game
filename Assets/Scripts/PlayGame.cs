using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class PlayGame : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(Menu.Zumba);

    }
}
