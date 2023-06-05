using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject objectToActivate;
    public int GameToPlay;

    public void ActivateObject()
    {
        objectToActivate.SetActive(true);
        Menu.Zumba = GameToPlay;
    }
}



