using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "Scriptable Object/Cutscene", fileName = "Cutscene", order = int.MaxValue - 1)]
public class Cutscene : ScriptableObject
{
    [SerializeField] public PlayableAsset playableAsset;
    [SerializeField] public int[] dialogueSkipIndex;
    [SerializeField] public double[] cutsceneSkipTime;
    [SerializeField] public string[] cutsceneSkipSound;
    [SerializeField] public string[] cutsceneParticipants;
}
