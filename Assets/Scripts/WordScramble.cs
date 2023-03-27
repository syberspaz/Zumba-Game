using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WordScramble : MonoBehaviour
{
    public char[] current;
    public string answer;
    public string temp;
    public WSController controller;
    float score = 200;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (score > 50)
            score -= 2 * Time.deltaTime;
        temp = new string(current);
        if (answer.Equals(temp))
        {
            //controller.GenerateWord();
            Menu.points += (int)score;
            SceneManager.LoadScene(Menu.background);
        }
    }
}
