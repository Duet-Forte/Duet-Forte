using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    private GameObject cutscenePlayer;
    private PlayableDirector director;
    private int currentCutsceneID;
    private bool isTalking;
    private double loopStartTime;
    private bool isCutscenePlaying;
    [SerializeField] private PlayableAsset[] cutscenes;
    [SerializeField] private Image fadeOutPanel;
    [SerializeField] private int[] dialogueSkipIndex;
    [SerializeField] private double[] cutsceneSkipTime;
    [SerializeField] private string[] cutsceneSkipSound;
    private int skipIndex;
    private Dictionary<string, PlayableAsset> cutsceneDictionary;
    public bool isPlaying { get => isCutscenePlaying; }
    public void InitSettings()
    {
        director = GetComponent<PlayableDirector>();
        cutsceneDictionary = new Dictionary<string, PlayableAsset>();
        foreach (var cutscene in cutscenes)
        {
            cutsceneDictionary.Add(cutscene.name, cutscene);
        }
        StartCutscene("TimmyHouse");
        cutscenePlayer = GameManager.FieldManager.Field.GetCutsceneObject("Zio");
        GameObject pitch = GameManager.FieldManager.Field.GetCutsceneObject("Pitch");
        GameObject timmy = GameManager.FieldManager.Field.GetCutsceneObject("Timmy");
        BindCutscene(director, "Player", cutscenePlayer);
        BindCutscene(director, "Pitch", pitch);
        BindCutscene(director, "Timmy", timmy);
        BindCutscene(director, "Camera", GameManager.FieldManager.Field.CutsceneCam.gameObject);
        GameManager.FieldManager.Field.SetCameraPath("Tutorial");
        GameManager.InputController.BindPlayerInputAction(Util.CustomEnum.PlayerAction.Skip, SkipCutscene);
        director.Play();
    }
    public void StartCutscene(string cutsceneName)
    {
        isCutscenePlaying = true;
        director.playableAsset = cutsceneDictionary[cutsceneName];
    }
    public async void Talk()
    {
        Debug.Log("¸» ½ÃÀÛ");
        Debug.Log(director.time);
        isTalking = true;
        await DialogueManager.Instance.Talk("Cutscene");
        DataBase.Dialogue.SetID("Cutscene", ++currentCutsceneID);
        isTalking = false;
        director.Play();
    }
    [ContextMenu("DEBUG/SpawnPlayer")]
    public void SpawnPlayer()
    {
        FadeIn(0.7f, () => {
        FadeOut(0.5f);
        GameManager.FieldManager.SpawnPlayer(cutscenePlayer.transform.position);
        GameManager.FieldManager.Field.GetEntity("Timmy").InitSettings("Timmy", GameManager.FieldManager.Field.GetCutsceneObject("Timmy").transform.position);
        GameManager.CameraManager.SetFollowCamera();
        GameManager.FieldManager.CheckPoint();
        isCutscenePlaying = false;
        GameManager.Storage.isCutscenePlaying = false;
        director.Stop();
        DialogueManager.Instance.EndDialogue();
        GameManager.FieldManager.Field.DisableCutsceneObjects();
        });
    }

    public void LoopStartPoint()
    {
        if(isTalking)
            loopStartTime = director.time;
    }
    public void LoopEndPoint()
    {
        if (isTalking)
        {
            director.time = loopStartTime;
            return;
        }
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
        GameManager.Instance.SetBattleScene("Timmy");
    }

    public void PauseDirector()
    {
        GameManager.Storage.isCutscenePlaying = true;
        double currentTime = director.time;
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        director.time = currentTime;
        director.Evaluate();
    }
    public void ReplayCutscene()
    {
        if (!isCutscenePlaying)
            return;
        double currentTime = director.time;
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        director.time = currentTime;
        director.Evaluate();
    }
    public virtual void BindCutscene(PlayableDirector director, string trackGroupName, GameObject bindObject)
    {
        var timeline = director.playableAsset as TimelineAsset;
        foreach (var groupTrack in timeline.GetRootTracks())
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
        GameManager.MusicChanger.SetMusic(name);
    }

    private void SkipCutscene(InputAction.CallbackContext context)
    {
        if (skipIndex >= cutsceneSkipTime.Length)
            return;
        isTalking = false;
        director.Pause();
        DialogueManager.Instance.EndDialogue();
        if(cutsceneSkipTime.Length > skipIndex)
            director.time = cutsceneSkipTime[skipIndex];
        if (dialogueSkipIndex.Length > skipIndex)
        {
            DataBase.Dialogue.SetID("Cutscene", dialogueSkipIndex[skipIndex]);
            currentCutsceneID = dialogueSkipIndex[skipIndex] + 1;
        }
        if (cutsceneSkipSound.Length > skipIndex)
            PlaySound(cutsceneSkipSound[skipIndex]);
        if(skipIndex == cutsceneSkipTime.Length - 1)
        {
            director.Evaluate();
            director.Play();
            DialogueManager.Instance.SkipEvent(DataBase.Dialogue.GetDialogue("Cutscene"), "Cutscene");
            DataBase.Dialogue.SetID("Cutscene", currentCutsceneID);
        }
        else
        {
            FadeIn(0.5f, () => { AfterSkipFadeInFinish().Forget(); });
        }
        skipIndex++;
    }

    private async UniTask AfterSkipFadeInFinish()
    {
        director.Evaluate();
        await UniTask.WaitForSeconds(1);
        director.Play();
        DialogueManager.Instance.SkipEvent(DataBase.Dialogue.GetDialogue("Cutscene"), "Cutscene");
        DataBase.Dialogue.SetID("Cutscene", currentCutsceneID);
        FadeOut(0.5f);
    }
}
