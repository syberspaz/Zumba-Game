using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.Kinect.Sensor;

public class KinectCameraToUI : MonoBehaviour
{
    public RawImage rawImage; // Assign this in the Unity Inspector
    private Device kinect;
    private Texture2D colorTexture;
    private byte[] imageData;

    void Start()
    {
        // Initialize Kinect
        try
        {
            kinect = Device.Open(0);
            kinect.StartCameras(new DeviceConfiguration
            {
                ColorFormat = ImageFormat.ColorBGRA32,
                ColorResolution = ColorResolution.R1080p,
                DepthMode = DepthMode.Off,
                CameraFPS = FPS.FPS30
            });

            // Create a Texture2D for the camera feed
            colorTexture = new Texture2D(1920, 1080, TextureFormat.BGRA32, false);
            rawImage.texture = colorTexture;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to initialize Kinect: " + e.Message);
        }
    }

    void Update()
    {
        if (kinect != null)
        {
            using (Capture capture = kinect.GetCapture())
            {
                Microsoft.Azure.Kinect.Sensor.Image colorImage = capture.Color;
                if (colorImage != null)
                {
                    if (imageData == null || imageData.Length != colorImage.Size)
                        imageData = new byte[colorImage.Size];

                    imageData = colorImage.Memory.ToArray();
                    colorTexture.LoadRawTextureData(imageData);
                    colorTexture.Apply();
                }
            }
        }
    }

    void OnDestroy()
    {
        if (kinect != null)
        {
            kinect.Dispose();
        }
    }
}
