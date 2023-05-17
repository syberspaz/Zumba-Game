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
    public bool isSelected = false;
    public KinectHand2D kHand;
    RectTransform rectTransform;
    // Start is called before the first frame update

    void Start() {
        back = transform.GetChild(0).gameObject;
        isFront = false;
        locked = false;
        rectTransform = GetComponent<RectTransform>();

    }

    void Update()
    {
        float dist = Vector2.Distance(kHand.rectTransform.localPosition, rectTransform.localPosition);

        if (isFront)
        {
            back.SetActive(false);
        }
        else if (!isFront)
        {
            back.SetActive(true);
        }
        if (dist < rectTransform.rect.width / 2f)
        {
            // kHand.updateSelection();
            if (kHand.handIsClosed && !isFront && !locked)
            {
                cardManager.FlipCard(this.gameObject);
                isFront = true;
                isSelected = true;
            }


        }
    }
        public void OnPointerClick(PointerEventData pointerEventData)
        {

            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                if (!locked && !isFront)
                {
                    cardManager.FlipCard(this.gameObject);
                    isFront = true;
                }

            }
        }
}

