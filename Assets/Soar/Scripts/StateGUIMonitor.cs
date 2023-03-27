using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGUIMonitor : MonoBehaviour
{
    public SoarSDK.VolumetricRender Renderer;
    public UnityEngine.UI.Text TextBox;
    public string NoInstanceText = "<no instance>";

    private SoarSDK.PlaybackInstance instance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (instance != Renderer.Instance)
        {
            instance = Renderer.Instance;
        }

        if (instance != null && TextBox != null)
        {
            TextBox.text = instance.PlaybackState.ToString();
        }
        else if (TextBox != null)
        {
            TextBox.text = NoInstanceText;
        }
    }
}