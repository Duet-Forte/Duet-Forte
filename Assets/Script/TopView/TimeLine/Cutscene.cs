using System.Runtime.InteropServices;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public abstract class Cutscene
{
    private string cutsceneName;
    private PlayableAsset somePlayableAsset;
    public abstract void InitSettings();

    public virtual void BindCutscene(PlayableDirector director, string trackGroupName, GameObject bindObject)
    {
        var timeline = director.playableAsset as TimelineAsset;
        foreach(GroupTrack groupTrack in timeline.GetRootTracks())
        {
            if (groupTrack.name != trackGroupName)
                continue;
            foreach(var track in groupTrack.GetChildTracks())
            {
                director.SetGenericBinding(track, bindObject);
            }
        }
    }
}
