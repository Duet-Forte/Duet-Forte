using System;
using UnityEngine;

public class CurrentSkillWindow : Menu
{
    public event Action<int> onPressed;
    public event Action<int> onSelected;
    public event Action<int> onDeselected;
    public override void InitSettings(MenuSelector selector, int index)
    {
        base.InitSettings(selector, index);
        SkillSaver skillSelector = selector as SkillSaver;
        onPressed -= skillSelector.RemoveCurrentSkill;
        onPressed += skillSelector.RemoveCurrentSkill;
        onSelected -= skillSelector.ShowSkillDescriptionInWindow;
        onSelected += skillSelector.ShowSkillDescriptionInWindow;
        onDeselected -= skillSelector.HideSkillDescription;
        onDeselected += skillSelector.HideSkillDescription;
    }
    public override void OnPressed()
    {
        onPressed?.Invoke(index);
    }

    public override void OnSelected()
    {
        onSelected?.Invoke(index);
    }

    public override void OnDeselected()
    {
        onDeselected?.Invoke(index);
    }
}
