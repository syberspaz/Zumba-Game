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

                // Store the current hit object as the last hit object
                lastHit = cursorHit;

                hoverTimer += Time.deltaTime;

                Debug.Log("Hit object");


                findMeManager.target.GetComponent<FindMeObject>().clicked = true;

                if (findMeManager.target.GetComponent<FindMeObject>().clicked = true)
                {
                    Debug.Log("Found object");
                }


                //findMeManager.target.GetComponent<FindMeObject>().clicked = false;

            }
        }
        else
        {
            hoverTimer -= Time.deltaTime;
            lastHit = new RaycastHit();

        }
        hoverTimer = Mathf.Clamp(hoverTimer, 0, hoverTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!hasEntered)
        {
            // Perform actions when entering the range of the object
            Debug.Log("Entered range of object: " + other.name);

            // Set the flag to indicate that the trigger has been entered
            hasEntered = true;
        }

    }
}
