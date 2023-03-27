using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FindMeHub : MonoBehaviour
{
    public static int target;
    // Start is called before the first frame update
    void Start()
    {
        target = Random.Range(1, 7);
        int scene;
        scene = Random.Range(8, 12);
        SceneManager.LoadScene(scene);
    }
}
