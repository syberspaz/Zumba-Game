using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FlipCard : MonoBehaviour, IPointerClickHandler {
  public CardManager cardManager;
  public GameObject back;
  public bool isFront;
  public bool locked = false;
  // Start is called before the first frame update
  void Start() {
    back = transform.GetChild(0).gameObject;
    isFront = false;
    locked = false;
  }

  void Update() {
    if (isFront) {
      back.SetActive(false);
    } else if (!isFront) {
      back.SetActive(true);
    }

  }

  public void OnPointerClick(PointerEventData pointerEventData) {
    if (pointerEventData.button == PointerEventData.InputButton.Left) {
      if (!locked && !isFront) {
        cardManager.FlipCard(this.gameObject);
        isFront = true;
      }
    }
  }
}
