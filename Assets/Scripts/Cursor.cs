using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cursor : MonoBehaviour
{
    public Transform handPos;
    private FindMeManager findMeManager;
    public Image cursor;
    public Camera cam;
    public bool isCol;
    public bool isHover;
    public float hoverTimer = 0.0f;
    public float hoverTime = 2.0f;
    public LayerMask cursorMask;

    // Start is called before the first frame update
    void Start()
    {
        findMeManager = FindObjectOfType<FindMeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(handPos.position);
        cursor.gameObject.transform.position = screenPos;
        Ray cursorRay;
        RaycastHit cursorHit;
        cursorRay = cam.ScreenPointToRay(screenPos);
        Debug.DrawRay(cursorRay.origin, cursorRay.direction * 10);
        if (Physics.Raycast(cursorRay, out cursorHit, 10000, cursorMask))
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= hoverTime)
            {
                /*findMeManager.GetComponent<FindMeObject>().clicked = true;

                if (findMeManager.target.GetComponent<FindMeObject>().clicked)
                {
                    Debug.Log("Object Selected");
                }*/
                
            }
        }
        else
        {
            hoverTimer -= Time.deltaTime;
        }
        hoverTimer = Mathf.Clamp(hoverTimer, 0, hoverTime);
    }
}
