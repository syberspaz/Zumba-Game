using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMeObject : MonoBehaviour
{
  public string hint;
  public bool clicked;
  public void OnMouseDown() {
    clicked = true;
  }
}
