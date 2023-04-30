using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AverageRecentRating : MonoBehaviour
{
  public float average;
  public int jointCount;
  public float interval;
  float timer;
  public Image image;
  public List<Sprite> spriteList;

  private void Update() {
    timer += Time.deltaTime;
    if (timer > interval) {
      float temp = average / jointCount;
      int score = 0;
      while (temp > 0) {
        score += 1;
        temp -= 0.2f;
      }
      image.sprite = spriteList[score];
      image.color = Color.white;
    }
  }
  private void FixedUpdate() {
    if (image.color.a > 0) {
      Color temp = image.color;
      temp.a -= 0.005f;
      image.color = temp;
    }
  }
}
