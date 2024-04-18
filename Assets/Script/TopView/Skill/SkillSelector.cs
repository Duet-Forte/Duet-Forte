using Util;
using UnityEngine;

public class SkillSelector : MenuSelector
{
    private SkillSaver skillSaver;
    private SkillScroll skillScroll;



    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int changedIndex = currentIndex - 1;
            if (changedIndex / Const.CONTENT_IN_ROW == currentIndex / Const.CONTENT_IN_ROW)
                SetIndex(changedIndex);
            menuArray[currentIndex].OnSelected();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int changedIndex = currentIndex + 1;
            if (changedIndex / Const.CONTENT_IN_ROW == currentIndex / Const.CONTENT_IN_ROW)
            {
                SetIndex(changedIndex);
                menuArray[currentIndex].OnSelected();
            }

        }
        else if (Input.GetKeyDown(KeyCode.Return))
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
