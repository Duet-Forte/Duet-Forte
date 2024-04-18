using System;
using UnityEngine;

public class CurrentSkillWindow : Menu
{
    public event Action<int> onPressed;

    public override void InitSettings(MenuSelector selector, int index)
    {
        base.InitSettings(selector, index);
        SkillSaver skillSelector = selector as SkillSaver;
        onPressed -= skillSelector.ChangeCurrentSkillWindowIndex;
        onPressed += skillSelector.ChangeCurrentSkillWindowIndex;
        skillSelector = null;
    }
    public override void OnPressed()
    {
        onPressed?.Invoke(index);
    }
}
