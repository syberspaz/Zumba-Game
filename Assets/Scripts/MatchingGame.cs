using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingGame : MonoBehaviour
{
    public int count;
    public Text card1;
    public Text card2;
    public bool locked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 2)
        {
            if (card1.text == card2.text)
            {
                locked = true;
            }
            count = 0;
        }
    }
}
