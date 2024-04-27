using Util;
using UnityEngine;
using System;

public class SkillSelector : MenuSelector
{
    private SkillSetter skillScroll;

    public void InitSettings(SkillSetter skillScroll)
    {
        base.InitSetting();
        this.skillScroll = skillScroll;
        menuArray[currentIndex].OnSelected();
    }

    public void Update()
    {
        if (!skillScroll.IsSelectorSelected)
            return;

        int changedIndex = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            changedIndex = currentIndex - 1;
            if(previousIndex == -1)
            {
                menuArray[currentIndex].OnSelected();
                previousIndex = currentIndex;
                return;
            }
            else if (changedIndex >= 0 && changedIndex / Const.CONTENT_IN_ROW == currentIndex / Const.CONTENT_IN_ROW)
                SetIndex(changedIndex);
            menuArray[currentIndex].OnSelected();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            changedIndex = currentIndex + 1;
            if (changedIndex < menuArray.Length && changedIndex / Const.CONTENT_IN_ROW == currentIndex / Const.CONTENT_IN_ROW)
                SetIndex(changedIndex);
            else
            {
                skillScroll.IsSelectorSelected = false;
                menuArray[currentIndex].OnDeselected();
                previousIndex = -1;
                return;
            }
            menuArray[currentIndex].OnSelected();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            changedIndex = currentIndex - Const.CONTENT_IN_ROW;
            if (changedIndex >= 0)
                SetIndex(changedIndex);
            menuArray[currentIndex].OnSelected();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            changedIndex = currentIndex + Const.CONTENT_IN_ROW;
            if (changedIndex < menuArray.Length)
                SetIndex(changedIndex);
            menuArray[currentIndex].OnSelected();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            menuArray[currentIndex].OnPressed();
        }
    }

    public override void SetIndex(int index)
    {
        previousIndex = currentIndex;
        menuArray[previousIndex].OnDeselected();
        currentIndex = index;
    }

    public void MoveToSkillSaver()
    {
        menuArray[currentIndex].OnDeselected();
    }
}
