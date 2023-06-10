using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StandingOrSitting : MonoBehaviour
{
    public GameObject objectToActivate;

    public void SetBoolTrue()
    {
        Menu.isStanding = true;
        objectToActivate.SetActive(true);
    }

    public void SetBoolFalse()
    {
        Menu.isStanding = false;
        objectToActivate.SetActive(true);
    }
}
