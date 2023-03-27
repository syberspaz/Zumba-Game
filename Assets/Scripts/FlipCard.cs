using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FlipCard : MonoBehaviour, IPointerClickHandler
{
    public MatchingGame script;
    public GameObject front;
    public GameObject back;
    public bool isFront;
    public bool locked = false;
    // Start is called before the first frame update
    void Start()
    {
        isFront = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFront)
        {
            back.SetActive(false);
        }
        else if (!isFront)
        {
            back.SetActive(true);
        }
        //if (!locked && !back.gameObject.activeInHierarchy)
        //{
        //    locked = script.locked;
        //    script.locked = false;
        //}
    }
    private void OnMouseDown()
    {
        //isFront = !isFront;
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (!locked && !isFront)
            {
                isFront = true;
                if (script.count == 0)
                {
                    script.card1.text = gameObject.name;
                    script.count++;
                }
                else if (script.count == 1)
                {
                    script.card2.text = gameObject.name;
                    script.count++;
                }
                else if (script.count == 2 && !script.locked)
                {
                    back.SetActive(true);
                }
            }
        }
    }
}
