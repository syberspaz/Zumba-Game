using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WordScramble : MonoBehaviour {
  public char[] current;
  public string answer;
  public string temp;
  public WSController controller;
  float timer;


  // Update is called once per frame
  void Update() {
    timer += Time.deltaTime;

    temp = new string(current);
    if (answer.Equals(temp)) {
      Score.wordScrambleTime = timer;
      SceneManager.LoadScene(Menu.background);
    }
  }
}
