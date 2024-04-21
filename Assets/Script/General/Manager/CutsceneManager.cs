using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    private GameObject cutscenePlayer;
    private PlayableDirector director;
    private int currentCutsceneID;
    private bool isReachedLoopEndPoint;
    private bool isTalking;
    private double loopStartTime;
    private Sequence fadeInAndOut;
    [SerializeField] private PlayableAsset[] cutscenes;
    [SerializeField] private Image fadeOutPanel;
    private Dictionary<string, PlayableAsset> cutsceneDictionary;

    public void InitSettings()
    {
        director = GetComponent<PlayableDirector>();
        cutsceneDictionary = new Dictionary<string, PlayableAsset>();
        foreach(var cutscene in cutscenes)
        {
            cutsceneDictionary.Add(cutscene.name, cutscene);
        }
        StartCutscene("TimmyHouse");
        cutscenePlayer = SceneManager.Instance.FieldManager.Field.GetCutsceneObject("CutscenePlayer");
        GameObject pitch = SceneManager.Instance.FieldManager.Field.GetCutsceneObject("Pitch");
        GameObject timmy = SceneManager.Instance.FieldManager.Field.GetCutsceneObject("Timmy");
        BindCutscene(director, "Player", cutscenePlayer);
        BindCutscene(director, "Pitch", pitch);
        BindCutscene(director, "Timmy", timmy);
        isReachedLoopEndPoint = false;
        director.Play();
    }
    public void StartCutscene(string cutsceneName)
    {
        director.playableAsset = cutsceneDictionary[cutsceneName];
    }
    public async void Talk()
    {
        isTalking = true;
        await DialogueManager.Instance.Talk("Cutscene", currentCutsceneID);
        ++currentCutsceneID;
        isTalking = false;
        director.Play();
    }

    public void SpawnPlayer()
    {
        cutscenePlayer.SetActive(false);
        SceneManager.Instance.FieldManager.SpawnPlayer(cutscenePlayer.transform.position);
    }
    public void Loop()
    {
        if (!isTalking)
        {
            isReachedLoopEndPoint = false;
            return;
        }

        if (isReachedLoopEndPoint && director.time != loopStartTime)
            director.time = loopStartTime;
        else
            loopStartTime = director.time;

        isReachedLoopEndPoint = true;
    }
    public void FadeOut()
    {
        if(fadeInAndOut == null)
        {
            Tween fadeIn = fadeOutPanel.DOFade(1, 1f);
            Tween fadeOut = fadeOutPanel.DOFade(0, 1f);
            fadeInAndOut = DOTween.Sequence();
            fadeInAndOut.Append(fadeIn).Append(fadeOut);
        }
        fadeInAndOut.Play();
    }
    public virtual void BindCutscene(PlayableDirector director, string trackGroupName, GameObject bindObject)
    {
        var timeline = director.playableAsset as TimelineAsset;
        foreach (GroupTrack groupTrack in timeline.GetRootTracks())
        {
            if (groupTrack.name != trackGroupName)
                continue;
            foreach (var track in groupTrack.GetChildTracks())
            {
                switch (track)
                {
                    case ActivationTrack activationTrack:
                        director.SetGenericBinding(activationTrack, bindObject);
                        break;
                    case AnimationTrack animationTrack:
                        director.SetGenericBinding(animationTrack, bindObject.GetComponent<Animator>());
                        break;
                    case SignalTrack signalTrack:

                        break;
                }
            }
        }
    }
}
