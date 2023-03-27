using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LetterDrag : MonoBehaviour
{
    public Canvas canvas;
    public bool locked = false;
    public bool placed = false;
    public int slotNum = 0;
    public bool colliding = false;
    Collider2D other = new Collider2D();
    public WordScramble ws;
    public TextMeshProUGUI letter;
    public bool isSelected;

    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out pos);

        transform.position = canvas.transform.TransformPoint(pos);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slot")
        {
            colliding = true;
            other = collision;
        }
        if (collision.gameObject.tag == "Cursor")
        {

        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slot")
        {
            colliding = false;
            other = new Collider2D();
            placed = false;
            slotNum = 0;
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && other != null)
        {
            transform.position = other.gameObject.transform.position;
            placed = true;
            slotNum = int.Parse(other.gameObject.name);
            ws.current[slotNum - 1] = char.Parse(letter.text);
        }
    }
}
