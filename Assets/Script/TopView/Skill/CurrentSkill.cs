using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrentSkill : Menu
{
    [SerializeField] private Image frame;
    [SerializeField] private Color selectedColor, defaultColor;
    public event Action<int> onPressed;
    public event Action<int> onSelected;

    public override void OnPressed()
    {
        onPressed?.Invoke(index);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        frame.color = selectedColor;
        onSelected?.Invoke(index);
    }

    public override void OnDeselected()
    {
        frame.color = defaultColor;
    }

    public void BindSaverEvent(SkillSaver skillSaver)
    {
        onSelected -= skillSaver.ShowSkillDescription;
        onSelected += skillSaver.ShowSkillDescription;
        onPressed -= skillSaver.SelectSkill;
        onPressed += skillSaver.SelectSkill;
    }
}
