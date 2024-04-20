using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : MonoBehaviour
{
    private GameObject cutscenePlayer;
    private PlayableDirector director;
    private int currentCutsceneID;
    [SerializeField] private PlayableAsset[] cutscenes;
    private Dictionary<string, PlayableAsset> cutsceneDictionary;

    public void InitSettings()
    {
        director = GetComponent<PlayableDirector>();
        cutsceneDictionary = new Dictionary<string, PlayableAsset>();
        foreach(var cutscene in cutscenes)
        {
            cutsceneDictionary.Add(cutscene.name, cutscene);
        }
        director.playableAsset = cutscenes[0];
        cutscenePlayer = SceneManager.Instance.FieldManager.Field.GetCutsceneObject("CutscenePlayer");
        BindCutscene(director, "Player", cutscenePlayer);
        director.Play();
    }
    public void StartCutscene(string cutsceneName)
    {
        director.playableAsset = cutsceneDictionary[cutsceneName];
    }
    public async void Talk()
    {
        director.Pause();
        await DialogueManager.Instance.Talk("Cutscene", currentCutsceneID);
        ++currentCutsceneID;
        director.Play();
    }

    public void SpawnPlayer()
    {
        cutscenePlayer.SetActive(false);
        SceneManager.Instance.FieldManager.SpawnPlayer(cutscenePlayer.transform.position);
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
