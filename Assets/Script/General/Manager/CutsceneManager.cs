using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using System;

public class CutsceneManager : MonoBehaviour
{
    private GameObject cutscenePlayer;
    private PlayableDirector director;
    private int currentCutsceneID;
    private bool isReachedLoopEndPoint;
    private bool isTalking;
    private double loopStartTime;
    [SerializeField] private PlayableAsset[] cutscenes;
    [SerializeField] private Image fadeOutPanel;
    private Dictionary<string, PlayableAsset> cutsceneDictionary;

    public void InitSettings()
    {
        director = GetComponent<PlayableDirector>();
        cutsceneDictionary = new Dictionary<string, PlayableAsset>();
        foreach (var cutscene in cutscenes)
        {
            cutsceneDictionary.Add(cutscene.name, cutscene);
        }
        StartCutscene("TimmyHouse");
        cutscenePlayer = SceneManager.Instance.FieldManager.Field.GetCutsceneObject("Zio");
        GameObject pitch = SceneManager.Instance.FieldManager.Field.GetCutsceneObject("Pitch");
        GameObject timmy = SceneManager.Instance.FieldManager.Field.GetCutsceneObject("Timmy");
        BindCutscene(director, "Player", cutscenePlayer);
        BindCutscene(director, "Pitch", pitch);
        BindCutscene(director, "Timmy", timmy);
        BindCutscene(director, "Camera", SceneManager.Instance.FieldManager.Field.CutsceneCam.gameObject);
        SceneManager.Instance.FieldManager.Field.SetCameraPath("Tutorial");
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
        await DialogueManager.Instance.Talk("Cutscene");
        DataBase.Instance.Dialogue.SetID("Cutscene", ++currentCutsceneID);
        isTalking = false;
        director.Play();
    }
    [ContextMenu("DEBUG/SpawnPlayer")]
    public void SpawnPlayer()
    {
        SceneManager.Instance.Storage.isCutscenePlaying = false;
        director.Stop();
        DialogueManager.Instance.SkipDialogue();
        cutscenePlayer.SetActive(false);
        SceneManager.Instance.FieldManager.SpawnPlayer(cutscenePlayer.transform.position);
        SceneManager.Instance.FieldManager.Field.GetEntity("Timmy").InitSettings("Timmy", SceneManager.Instance.FieldManager.Field.GetCutsceneObject("Timmy").transform.position);
        SceneManager.Instance.CameraManager.SetFollowCamera();
        SceneManager.Instance.FieldManager.CheckPoint();
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
    public void FadeIn(float time)
    {
        fadeOutPanel.DOFade(1, time);
    }
    public void FadeOut(float time)
    {
        fadeOutPanel.DOFade(0, time);
    }
    public void FadeIn(float time, Action onFadeFinish = null)
    {
        Tween fadeIn = fadeOutPanel.DOFade(1, time).OnComplete(() =>
        {
            onFadeFinish?.Invoke();
        });
    }
    public void FadeOut(float time, Action onFadeFinish = null)
    {
        Tween fadeOut = fadeOutPanel.DOFade(0, time).OnComplete(() =>
        {
            onFadeFinish?.Invoke();
        });
    }
    [ContextMenu("DEBUG/StartBattle")]
    public void EnterBattle()
    {
        double currentTime = director.time;
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        director.time = currentTime;
        director.Evaluate();
        SceneManager.Instance.SetBattleScene("Timmy");
    }

    public void PauseDirector()
    {
        SceneManager.Instance.Storage.isCutscenePlaying = true;
        double currentTime = director.time;
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        director.time = currentTime;
        director.Evaluate();
    }
    public void ReplayCutscene()
    {
        double currentTime = director.time;
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        director.time = currentTime;
        director.Evaluate();
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
                    default:
                        break;
                }
            }
        }
    }

    public void PlaySound(string name)
    {
        SceneManager.Instance.MusicChanger.SetMusic(name);
    }
}
