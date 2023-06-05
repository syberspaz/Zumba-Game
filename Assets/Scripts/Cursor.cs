using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cursor : MonoBehaviour
{
    public GameObject handPos;
    private FindMeManager findMeManager;
    public Image cursor;
    public Camera cam;
    public bool isCol;
    public bool isHover;
    private RaycastHit lastHit;
    public float hoverTimer = 0.0f;
    public float hoverTime = 2.0f;
    public LayerMask cursorMask;
    private bool hasEntered = false;
    private bool isClicking = false;
    // Start is called before the first frame update
    void Start()
    {
        handPos = GameObject.Find("rightHand");
        findMeManager = FindObjectOfType<FindMeManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(handPos.transform.position);
        cursor.gameObject.transform.position = screenPos;
        Ray cursorRay;
        RaycastHit cursorHit;
        cursorRay = cam.ScreenPointToRay(screenPos);
        Debug.DrawRay(cursorRay.origin, cursorRay.direction * 10);
        if (Physics.Raycast(cam.transform.position, handPos.transform.position - cam.transform.position, out cursorHit, Mathf.Infinity, cursorMask))
        {
            if (lastHit.transform != cursorHit.transform)
            {
                hasEntered = false;
                lastHit = cursorHit;
                hoverTimer += Time.deltaTime;
                Debug.Log("Hit object: " + cursorHit.transform.name);

                if (cursorHit.transform == findMeManager.target.transform)
                {
                    if (!findMeManager.target.GetComponent<FindMeObject>().clicked)
                    {
                        findMeManager.target.GetComponent<FindMeObject>().clicked = true;
                        Debug.Log("Found object");
                    }
                }
                else
                {
                    // Increment error count for other objects
                    bool foundClickedObject = false;
                    for (int i = 0; i < findMeManager.findMeObjects.Count; i++)
                    {
                        if (findMeManager.findMeObjects[i] != findMeManager.target && findMeManager.findMeObjects[i].GetComponent<FindMeObject>().clicked && hasEntered)
                        {
                            foundClickedObject = true;
                            break;
                        }
                    }

                    if (!foundClickedObject)
                    {
                        findMeManager.errorCount++;
                    }
                }
            }
        }

        hoverTimer -= Time.deltaTime;
        lastHit = new RaycastHit();
        
        hoverTimer = Mathf.Clamp(hoverTimer, 0, hoverTime);
    }


    void OnTriggerEnter(Collider other)
    {
        if (!hasEntered)
        {
            // Perform actions when entering the range of the object
            Debug.Log("Entered range of object: " + other.name);

            // Set the flag to indicate that the trigger has been entered
            hasEntered = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        hasEntered = true;
    }
}

