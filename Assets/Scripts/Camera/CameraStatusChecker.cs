using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.Kinect.Sensor;

public class CameraStatusChecker : MonoBehaviour
{
    public UnityEngine.UI.Image statusIndicator; // UI Image to show connection status
    public Color connectedColor = Color.green; // Green when connected
    public Color disconnectedColor = Color.red; // Red when disconnected
    

    private void Start()
    {
        UpdateUI(SkeletalTrackingProvider.IsKinectConnected);
        //CheckCameraStatus();
    }

    private void Update()
    {
        UpdateUI(SkeletalTrackingProvider.IsKinectConnected);
        //CheckCameraStatus(); 
    }

    private void CheckCameraStatus()
    {
        bool isCameraConnected = false;
        try
        {
            // Attempt to open the first available device
            using (Device kinectDevice = Device.Open(0))
            {
                isCameraConnected = kinectDevice != null;
            }
        }
        catch
        {
            isCameraConnected = false;
        }

        // Update UI indicator based on connection status
        if (statusIndicator != null)
        {
            if (isCameraConnected)
            { 
                //this.gameObject.SetActive(false);
                statusIndicator.color = connectedColor; // Set to green if the camera is connected
                          
            }
            else
            {
                //this.gameObject.SetActive(true);
                //isConnected = true;
                statusIndicator.color = disconnectedColor; // Set to red if the camera is not connected
            }
        }
    }

    private void UpdateUI(bool isConnected)
    {
        if (isConnected)
        {
            statusIndicator.color = connectedColor; // Set to green if the camera is connected

        }
        else
        {
            statusIndicator.color = disconnectedColor; // Set to red if the camera is not connected
        }
    }


}
