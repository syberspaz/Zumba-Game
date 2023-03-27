using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour
{
    public Canvas canvas;
    public bool locked = false;
    public bool colliding = false;
    Collider2D other = new Collider2D();
    public Jigsaw manager;

    public void DragHandler(BaseEventData data)
    {
        if (!locked)
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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        colliding = true;
        other = collision;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        colliding = false;
        other = new Collider2D();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && other != null)
        {
            if (other.gameObject.name == gameObject.name && !locked)
            {
                transform.position = other.gameObject.transform.position;
                locked = true;
                manager.lockedCount++;
                Debug.Log("Locked");
            }
        }
    }
}
