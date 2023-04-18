using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecordingParts : MonoBehaviour
{
    public ZumbaPointList zpl;
    float timer;
    private void Start()
    {
        zpl.actionList = new List<Vector4>();
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        zpl.actionList.Add(new Vector4(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z, timer));
    }
}