using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using SoarSDK;
using UnityEngine.Timeline;

public class VolumetricTimelineGlobals : MonoBehaviour
{
    public static List<PlaybackInstance> instanceList = new List<PlaybackInstance>();
}

public class VolumetricRenderTrackMixer : PlayableBehaviour
{

    public PlaybackState state;
    public PlaybackInstancePlayState instanceState;
    public string currentFile;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        VolumetricRender volRender = playerData as VolumetricRender;
        PlaybackInstance instance = volRender.GetComponent<PlaybackInstance>();

        if (!instance) { return; }

        MaterialPropertyBlock props = instance.MaterialProps;

        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0f)
            {
                ScriptPlayable<VolumetricRenderBehavior> inputPlayable = (ScriptPlayable<VolumetricRenderBehavior>)playable.GetInput(i);
                VolumetricRenderBehavior input = inputPlayable.GetBehaviour();
                PlayableDirector director = playable.GetGraph().GetResolver() as PlayableDirector;
                state = volRender.GetComponent<PlaybackInstance>().PlaybackState;
                switch (state)
                {
                    case PlaybackState.BUFFERING:
                        Time.timeScale = 0;
                        break;
                    case PlaybackState.DECODE_CATCH_UP:
                        Time.timeScale = 0;
                        break;
                    default:
                        Time.timeScale = 1;
                        break;
                }
                if (director.state == PlayState.Playing)
                {
                    if (input.clipPlaying)
                    {
                        if (!input.seeking)
                        {
                            if (!input.clipStarted)
                            {
                                instance.Play();
                                input.clipStarted = true;
                            }

                            if(instance.PlayState == PlaybackInstancePlayState.Paused)
                            {
                                instance.Play();
                            }
                        }

                        if (input.seeking)
                        {
                            input.seeking = false;
                        }
                    }

                    if (!input.clipPlaying)
                    {
                        if (!input.seeking)
                        {
                            volRender.GetComponent<MeshRenderer>().GetPropertyBlock(props);
                            instance.MaterialProps.Clear();
                            volRender.GetComponent<MeshRenderer>().SetPropertyBlock(props);
                            volRender.LoadNewClip(input.fileName);
                            currentFile = input.fileName;
                            input.clipPlaying = true;
                        }

                        if (input.seeking)
                        {
                            instance.SeekToCursor((ulong)(inputPlayable.GetTime() * 1000000.0f));
                        }
                    }
                }

                if (director.state == PlayState.Paused)
                {
                    if (input.clipLoaded)
                    {
                        if (input.clipPlaying)
                        {
                            instance.SeekToCursor((ulong)(inputPlayable.GetTime() * 1000000.0f));
                            input.seeking = true;
                        }

                        if (!input.clipPlaying)
                        {
                            if(input.fileName != currentFile)
                            {
                                volRender.GetComponent<MeshRenderer>().GetPropertyBlock(props);
                                instance.MaterialProps.Clear();
                                volRender.GetComponent<MeshRenderer>().SetPropertyBlock(props);
                                volRender.LoadNewClip(input.fileName);
                                currentFile = input.fileName;
                                instance.SeekToCursor((ulong)(inputPlayable.GetTime() * 1000000.0f));
                                input.seeking = true;
                            }
                            else
                            {
                                instance.SeekToCursor((ulong)(inputPlayable.GetTime() * 1000000.0f));
                                input.seeking = true;
                            }
                        }
                    }
                }
            }

            if (inputWeight == 0f)
            {
                ScriptPlayable<VolumetricRenderBehavior> inputPlayable = (ScriptPlayable<VolumetricRenderBehavior>)playable.GetInput(i);
                VolumetricRenderBehavior input = inputPlayable.GetBehaviour();
                PlayableDirector director = playable.GetGraph().GetResolver() as PlayableDirector;

                if (director.state == PlayState.Paused)
                {
                    input.clipPlaying = false;
                }
            }
        }
    }
}
