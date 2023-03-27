using System;
using UnityEngine.Playables;
using SoarSDK;

[Serializable]
public class VolumetricRenderBehavior : PlayableBehaviour
{
    [VolumetricStreamFile]
    public string fileName;

    internal bool clipPlaying;
    internal bool clipLoaded;
    internal bool clipStarted;
    internal bool seeking;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        clipLoaded = true;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        clipLoaded = false;
    }
}
