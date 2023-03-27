using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jigsaw : MonoBehaviour
{
    public GameObject[] pieces = new GameObject[9];
    public Drag[] scripts = new Drag[9];
    public int lockedCount = 0;
    public bool isCompleted = false;
    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < pieces.Length; i++)
        //{
        //    scripts[i] = pieces[i].GetComponent<Drag>();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (lockedCount == 9)
        {
            isCompleted = true;
            Debug.Log("Done");
        }
    }
}
