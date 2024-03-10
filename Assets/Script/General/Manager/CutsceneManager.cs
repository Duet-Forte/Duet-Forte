using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject cutScenePlayer;
    private PlayableDirector director;
    private int currentCutsceneID;

    public async void Talk()
    {
        if(director == null)
            director = GetComponent<PlayableDirector>();
        director.Pause();
        await DialogueManager.Instance.Talk("Cutscene", currentCutsceneID);
        ++currentCutsceneID;
        director.Play();
    }

    public void SpawnPlayer()
    {
        cutScenePlayer.SetActive(false);
        SceneManager.Instance.FieldManager.SpawnPlayer(cutScenePlayer.transform.position);
    }
}
