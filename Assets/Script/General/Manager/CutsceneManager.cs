using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Util;

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
        director.Stop();
        DialogueManager.Instance.SkipDialogue();
        cutscenePlayer.SetActive(false);
        SceneManager.Instance.FieldManager.SpawnPlayer(cutscenePlayer.transform.position);
        SceneManager.Instance.CameraManager.SetFollowCamera();
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
        Tween fadeIn = fadeOutPanel.DOFade(1, time);
    }
    public void FadeOut(float time)
    {
        Tween fadeOut = fadeOutPanel.DOFade(0, time);
    }
    [ContextMenu("DEBUG/StartBattle")]
    public void EnterBattle()
    {
        AkSoundEngine.PostEvent("NonCombat_BGM_Stop", gameObject);
        director.Pause();
        SceneManager.Instance.SetBattleScene(null);
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
