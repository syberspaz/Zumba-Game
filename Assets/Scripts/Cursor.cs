using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cursor : MonoBehaviour
{
    public Transform handPos;
    public Transform FingerTip;
    public Transform Thumb;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(handPos.position);
        cursor.gameObject.transform.position = screenPos;
        Ray cursorRay;
        RaycastHit cursorHit;
        cursorRay = cam.ScreenPointToRay(screenPos);
        Vector3 MousePos = Input.mousePosition;
        MousePos = handPos.position;
        {
            Debug.Log(MousePos.x);
            Debug.Log(MousePos.y);
        }
        Debug.DrawRay(cursorRay.origin, cursorRay.direction * 10);
        if (Physics.Raycast(cursorRay, out cursorHit, 10000, cursorMask))
        {
            /*hoverTimer += Time.deltaTime;
            if (hoverTimer >= hoverTime)
            {
                Debug.Log("Hover Complete");
                if (FindMeHub.target.ToString() == cursorHit.transform.gameObject.name)
                {
                    Debug.Log("Object Selected");
                    //SceneManager.LoadScene(7); //load hub
                    SceneManager.LoadScene(Menu.background);
                }
            }*/
        }
        else
        {
            hoverTimer -= Time.deltaTime;
        }
        hoverTimer = Mathf.Clamp(hoverTimer, 0, hoverTime);
    }
}
