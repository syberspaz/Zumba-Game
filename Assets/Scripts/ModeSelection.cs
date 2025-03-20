using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ModeSelection : MonoBehaviour
{
    public GameObject objectToActivate;

    public void SetBoolTrue()
    {

        objectToActivate.SetActive(true);
    }
}
