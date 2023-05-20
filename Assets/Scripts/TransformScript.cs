using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformScript : MonoBehaviour
{
    public GameObject KinectTracker;
    public float xInput;
    public float yInput;
    public float zInput;
    public Quaternion kinectRotation;

    // Start is called before the first frame update
    void Start()
    {

        KinectTracker = GameObject.Find("Kinect4AzureTracker");

        KinectTracker.transform.position = new Vector3(xInput, yInput, zInput);

        KinectTracker.transform.rotation = kinectRotation;

    }


}
