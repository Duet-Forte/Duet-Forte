using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MenuSelector
{
    private void Awake()
    {
        AkSoundEngine.PostEvent("Ob_Fire_Fire_Amb", gameObject);
        InitSetting();
    }
    private void Update() //���� input System ���� ����.
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int changedIndex = currentIndex - 1;
            SetIndex(changedIndex);
            menuArray[currentIndex].OnSelected();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            int changedIndex = currentIndex + 1;
            SetIndex(changedIndex);
            menuArray[currentIndex].OnSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            menuArray[currentIndex].OnPressed();
        }
    }
}
