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
    private Cutscene currentPlayingCutscene;
    private int currentCutsceneID;
    private double loopStartTime;
    private bool isCutscenePlaying;
    private Cutscene[] cutscenes;
    [SerializeField] private Image fadeOutPanel;
    private int skipIndex;
    public bool isPlaying { get => isCutscenePlaying; }
    public void InitSettings(int saveInt)
    {
        director = GetComponent<PlayableDirector>();
        cutscenes = Resources.LoadAll<Cutscene>(Util.Const.CUTSCENE_PATH);

        if(saveInt == -1)
        {
            currentCutsceneID = 0;
            StartCutscene();
        }
        else
        {
            FadeOut(0.5f);
        }
    }
    public void StartCutscene()
    {
        string cutsceneName = cutscenes[currentCutsceneID].name;
        currentPlayingCutscene = cutscenes[currentCutsceneID];
        isCutscenePlaying = true;
        director.playableAsset = cutscenes[currentCutsceneID].playableAsset;
        GameManager.FieldManager.Field.GetEntity("Timmy").gameObject.SetActive(false);
        foreach (var participant in currentPlayingCutscene.cutsceneParticipants)
        {
            var participantObject = GameManager.FieldManager.Field.GetCutsceneObject(participant);
            BindCutscene(participant, participantObject);

            if (participant == "Zio")
                cutscenePlayer = participantObject;
        }
        BindCutscene("Camera", GameManager.FieldManager.Field.CutsceneCam.gameObject);
        GameManager.FieldManager.Field.SetCameraPath(cutsceneName);
        GameManager.InputController.BindPlayerInputAction(Util.CustomEnum.PlayerAction.Skip, SkipCutscene);
        director.Play();
    }
    public async void Talk()
    {
        Debug.Log("¸» ½ÃÀÛ");
        Debug.Log(director.time);
        await DialogueManager.Instance.Talk("Cutscene");
        DataBase.Dialogue.SetID("Cutscene", ++currentCutsceneID);
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
        if(DialogueManager.Instance.IsTalking)
            loopStartTime = director.time;
    }
    public void LoopEndPoint()
    {
        if (DialogueManager.Instance.IsTalking)
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
    public virtual void BindCutscene(string trackGroupName, GameObject bindObject)
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
        if (skipIndex >= currentPlayingCutscene.cutsceneSkipTime.Length || !isPlaying)
            return;
        director.Pause();
        DialogueManager.Instance.EndDialogue();
        if(currentPlayingCutscene.cutsceneSkipTime.Length > skipIndex)
            director.time = currentPlayingCutscene.cutsceneSkipTime[skipIndex];
        if (currentPlayingCutscene.dialogueSkipIndex.Length > skipIndex)
        {
            DataBase.Dialogue.SetID("Cutscene", currentPlayingCutscene.dialogueSkipIndex[skipIndex]);
            currentCutsceneID = currentPlayingCutscene.dialogueSkipIndex[skipIndex] + 1;
        }
        if (currentPlayingCutscene.cutsceneSkipSound.Length > skipIndex)
            PlaySound(currentPlayingCutscene.cutsceneSkipSound[skipIndex]);
        if(skipIndex == currentPlayingCutscene.cutsceneSkipTime.Length - 1)
        {
            director.Evaluate();
            director.Play();
            DialogueManager.Instance.SkipEvent(DataBase.Dialogue.GetDialogue("Cutscene"), "Cutscene", true);
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
        DialogueManager.Instance.SkipEvent(DataBase.Dialogue.GetDialogue("Cutscene"), "Cutscene", true);
        DataBase.Dialogue.SetID("Cutscene", currentCutsceneID);
        FadeOut(0.5f);
    }
}


