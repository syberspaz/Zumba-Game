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
  public float timer;
  public bool finished;

  // Update is called once per frame
  void Update() {
    temp = new string(current);
    if (answer.Equals(temp)) {
      finished = true;
    } else {
      timer += Time.deltaTime;
    }
  }
}
