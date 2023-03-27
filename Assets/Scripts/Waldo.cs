using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Waldo : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string hint;
    public int target;
    float score = 200;

    // Start is called before the first frame update
    void Start()
    {
        target = FindMeHub.target;
        if (FindMeHub.target.ToString() == gameObject.name)
        {
            text.text = hint;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (score > 50)
            score -= 2 * Time.deltaTime;
    }
    public void OnMouseDown()
    {
            if (FindMeHub.target.ToString() == gameObject.name)
            {
                //SceneManager.LoadScene(7); //load hub
                Menu.points = (int)score;
                SceneManager.LoadScene(Menu.background);
            }
    }
}
