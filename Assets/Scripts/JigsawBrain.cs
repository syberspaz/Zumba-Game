using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JigsawBrain : MonoBehaviour
{
    public Canvas beach;
    public float timer;
    public Canvas goat;
    public Canvas toronto;
    public Canvas flowers;
    public Canvas ship;
    public Canvas dog;
    public Canvas peggy;
    public Canvas boardwalk;
    public int selected;
    public Jigsaw beachScript;
    public Jigsaw goatScript;
    public Jigsaw torontoScript;
    public Jigsaw flowersScript;
    public Jigsaw shipScript;
    public Jigsaw dogScript;
    public Jigsaw peggyScript;
    public Jigsaw boardwalkScript;
    float score = 200;
    // Start is called before the first frame update
    void Start()
    {
        selected = Random.Range(1, 9);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (score > 50)
            score -= 2 * Time.deltaTime;

        if (beachScript.isCompleted)
        {
            beachScript.isCompleted = false;
            selected = Random.Range(1, 9);
            Menu.points += (int)score;
            SceneManager.LoadScene(Menu.Zumba);
            Score.JigsawTime = timer;
        }
        if (goatScript.isCompleted)
        {
            goatScript.isCompleted = false;
            selected = Random.Range(1, 9);
            Menu.points += (int)score;
            SceneManager.LoadScene(Menu.Zumba);
            Score.JigsawTime = timer;
        }
        if (torontoScript.isCompleted)
        {
            torontoScript.isCompleted = false;
            selected = Random.Range(1, 9);
            Menu.points += (int)score;
            SceneManager.LoadScene(Menu.Zumba);
            Score.JigsawTime = timer;
        }
        if (flowersScript.isCompleted)
        {
            flowersScript.isCompleted = false;
            selected = Random.Range(1, 9);
            Menu.points += (int)score;
            SceneManager.LoadScene(Menu.Zumba);
            Score.JigsawTime = timer;
        }
        if (shipScript.isCompleted)
        {
            shipScript.isCompleted = false;
            selected = Random.Range(1, 9);
            Menu.points += (int)score;
            SceneManager.LoadScene(Menu.Zumba);
            Score.JigsawTime = timer;
        }
        if (dogScript.isCompleted)
        {
            dogScript.isCompleted = false;
            selected = Random.Range(1, 9);
            Menu.points += (int)score;
            SceneManager.LoadScene(Menu.Zumba);
            Score.JigsawTime = timer;
        }
        if (peggyScript.isCompleted)
        {
            peggyScript.isCompleted = false;
            selected = Random.Range(1, 9);
            Menu.points += (int)score;
            SceneManager.LoadScene(Menu.Zumba);
            Score.JigsawTime = timer;
        }
        if (boardwalkScript.isCompleted)
        {
            boardwalkScript.isCompleted = false;
            selected = Random.Range(1, 9);
            Menu.points += (int)score;
            SceneManager.LoadScene(Menu.Zumba);
            Score.JigsawTime = timer;
        }
        switch (selected)
        {
            case 1:
                goat.gameObject.SetActive(false);
                boardwalk.gameObject.SetActive(false);
                dog.gameObject.SetActive(false);
                toronto.gameObject.SetActive(false);
                flowers.gameObject.SetActive(false);
                ship.gameObject.SetActive(false);
                peggy.gameObject.SetActive(false);
                beach.gameObject.SetActive(true);
                break;
            case 2:
                beach.gameObject.SetActive(false);
                boardwalk.gameObject.SetActive(false);
                dog.gameObject.SetActive(false);
                toronto.gameObject.SetActive(false);
                flowers.gameObject.SetActive(false);
                ship.gameObject.SetActive(false);
                peggy.gameObject.SetActive(false);
                goat.gameObject.SetActive(true);
                break;
            case 3:
                beach.gameObject.SetActive(false);
                goat.gameObject.SetActive(false);
                boardwalk.gameObject.SetActive(false);
                dog.gameObject.SetActive(false);
                flowers.gameObject.SetActive(false);
                ship.gameObject.SetActive(false);
                peggy.gameObject.SetActive(false);
                toronto.gameObject.SetActive(true);
                break;
            case 4:
                beach.gameObject.SetActive(false);
                goat.gameObject.SetActive(false);
                boardwalk.gameObject.SetActive(false);
                dog.gameObject.SetActive(false);
                toronto.gameObject.SetActive(false);
                ship.gameObject.SetActive(false);
                peggy.gameObject.SetActive(false);
                flowers.gameObject.SetActive(true);
                break;
            case 5:
                beach.gameObject.SetActive(false);
                goat.gameObject.SetActive(false);
                boardwalk.gameObject.SetActive(false);
                dog.gameObject.SetActive(false);
                toronto.gameObject.SetActive(false);
                flowers.gameObject.SetActive(false);
                peggy.gameObject.SetActive(false);
                ship.gameObject.SetActive(true);
                break;
            case 6:
                beach.gameObject.SetActive(false);
                goat.gameObject.SetActive(false);
                boardwalk.gameObject.SetActive(false);
                toronto.gameObject.SetActive(false);
                flowers.gameObject.SetActive(false);
                ship.gameObject.SetActive(false);
                peggy.gameObject.SetActive(false);
                dog.gameObject.SetActive(true);
                break;
            case 7:
                beach.gameObject.SetActive(false);
                goat.gameObject.SetActive(false);
                boardwalk.gameObject.SetActive(false);
                dog.gameObject.SetActive(false);
                toronto.gameObject.SetActive(false);
                flowers.gameObject.SetActive(false);
                ship.gameObject.SetActive(false);
                peggy.gameObject.SetActive(true);
                break;
            case 8:
                beach.gameObject.SetActive(false);
                goat.gameObject.SetActive(false);
                dog.gameObject.SetActive(false);
                toronto.gameObject.SetActive(false);
                flowers.gameObject.SetActive(false);
                ship.gameObject.SetActive(false);
                peggy.gameObject.SetActive(false);
                boardwalk.gameObject.SetActive(true);
                break;
        }
    }
}
