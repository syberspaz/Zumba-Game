using UnityEngine;
using SoarSDK;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class PlaybackControls : MonoBehaviour
{

    public VolumetricRender playbackComponent;
    public Slider scrubbingSlider;
    private bool getSliderHandle;
    [VolumetricStreamFile]
    public string newClipFileName;
    internal bool foundInstance;
    private PlaybackInstance instance;


    // Start is called before the first frame update
    void Start()
    {
        //Screen.SetResolution(Screen.width / 2, Screen.height / 2, true);
    }

    private void Update()
    {
        if (!foundInstance)
        {
            if (instance == null)
            {
                instance = playbackComponent.Instance;
            }

            else
            {
                foundInstance = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                VolumetricRender temp = hit.transform.gameObject.GetComponentInChildren<VolumetricRender>();
                if (temp != null) {
                    playbackComponent = temp;
                    instance = temp.Instance;
                }
            }
        }

        if(playbackComponent != null)
        {
            scrubbingSlider.maxValue = instance.FullDuration;

            if (!getSliderHandle)
            {
                scrubbingSlider.value = instance.CursorPosition;
            }
        }
    }

    public void SeekToTimestamp()
    {
        getSliderHandle = false;
        instance.SeekToCursor((ulong)scrubbingSlider.value);
        PlaybackStart();
    }

    public void GetSliderHandle()
    {
        getSliderHandle = true;
        PlaybackStop();
    }

    public void PlaybackStart()
    {
        instance.Play();
    }

    public void PlaybackPause()
    {
        instance.Pause();
    }

    public void PlaybackStop()
    {
        instance.Stop();
    }

    public void LoadNewClip()
    {
        playbackComponent.LoadNewClip(newClipFileName);
        instance = playbackComponent.Instance;
    }

    public void EnableLighting()
    {
        instance.EnableReLighting = !instance.EnableReLighting;
    }
}
