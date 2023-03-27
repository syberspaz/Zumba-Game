using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using SoarSDK;
using System.IO;
using UnityEngine.Video;

public class TimelineDirector : MonoBehaviour
{
    public TimelineAsset timelineAsset;
    public PlayableDirector director;
    internal bool ready;
    internal PlaybackInstance instance;
    internal int trackIndex = 0;
    internal int clipIndex = 0;
    internal int videoIndex = 0;

    [HideInInspector] public List<double> durationList;

    [HideInInspector] public List<VideoPlayer> playerList;

    private void Awake()
    {
        director.stopped += Director_stopped;
        director.paused += Director_paused;
    }

    private void Director_paused(PlayableDirector obj)
    {
        var outputTracks = timelineAsset.GetOutputTracks();

        foreach (var outputTrack in outputTracks)
        {
            if (outputTrack is VolumetricRenderTrack)
            {
                VolumetricRender volRender = director.GetGenericBinding(outputTrack) as VolumetricRender;
                volRender.GetComponent<PlaybackInstance>().Pause();
            }

        }
    }

    private void Director_stopped(PlayableDirector obj)
    {
        var outputTracks = timelineAsset.GetOutputTracks();
        foreach (var outputTrack in outputTracks)
        {
            if (outputTrack is VolumetricRenderTrack)
            {
                VolumetricRender volRender = director.GetGenericBinding(outputTrack) as VolumetricRender;
                volRender.GetComponent<PlaybackInstance>().Stop();
            }

        }
    }

    public void PrepareVideo(VideoPlayer videoPlayer, string path)
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = path;
        videoPlayer.prepareCompleted += Prepared;
        videoIndex++;
        videoPlayer.Prepare();
    }

    private void Update()
    {

        if (ready == false)
        {
            var outputTracks = timelineAsset.GetOutputTracks();
            foreach (var outputTrack in outputTracks)
            {
                if (outputTrack is VolumetricRenderTrack)
                {
                    trackIndex++;
                    var c = outputTrack.GetClips();
                    foreach (var clip in c)
                    {
                        VolumetricRenderClip testClip = clip.asset as VolumetricRenderClip;
                        string extention = Path.GetExtension(testClip.attributes.fileName);
                        string newClip = testClip.attributes.fileName.Replace("_master" + extention, ".mp4");
                        string fullPath = Application.streamingAssetsPath + "/" + newClip;
                        VideoPlayer videoPlayer = gameObject.AddComponent<VideoPlayer>();
                        videoPlayer.hideFlags = HideFlags.HideInInspector;
                        PrepareVideo(videoPlayer, fullPath);
                    }
                    ready = true;
                }
            }
        }
    }

    internal void Prepared(VideoPlayer videoPlayer)
    {
        durationList.Add(videoPlayer.length);
        playerList.Add(videoPlayer);

        if (durationList.Count() == videoIndex)
        {
            for (int i = 1; i <= trackIndex; i++)
            {
                var outputTrack = timelineAsset.GetOutputTrack(i);
                var clips = outputTrack.GetClips();

                foreach (var clip in clips)
                {
                    VolumetricRenderClip testClip = clip.asset as VolumetricRenderClip;
                    clip.duration = durationList.ElementAt(clipIndex);
                    clipIndex++;
                }
            }

            foreach (var player in playerList)
            {
                Destroy(player);
            }
            playerList = null;
            director.Play();
        }
    }
}
