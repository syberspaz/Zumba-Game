using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
  public static bool isRightHanded;
  public static int background = 2;
  public static int avatar;
  public static bool isStanding = true;
  public static int points = 0;
  public static int song = 0;
  // Start is called before the first frame update
  void Awake() {

  }
  void Start() {
    SceneManager.LoadScene(background);
  }

  // Update is called once per frame

}
