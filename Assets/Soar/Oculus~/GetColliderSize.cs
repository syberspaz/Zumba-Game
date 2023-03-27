using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetColliderSize : MonoBehaviour
{

    public BoxCollider collider;
    private bool getValues;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!getValues)
        {
            collider = gameObject.transform.GetChild(0).GetComponent<BoxCollider>();
            if(collider.center != new Vector3(0, 0, 0))
            {
                getValues = true;
                gameObject.GetComponent<BoxCollider>().center = new Vector3(collider.center.x, collider.center.y + collider.transform.position.y, collider.center.z);
                gameObject.GetComponent<BoxCollider>().size = collider.size;

            }
        }
    }
}
