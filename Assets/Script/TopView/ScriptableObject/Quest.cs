using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Object/Quest", order = 4)]
public class Quest : ScriptableObject
{
    public int ID;
    public string title;
    public string description;
    public int gold;
    public int experiencePoint;
    public int skillId;
}