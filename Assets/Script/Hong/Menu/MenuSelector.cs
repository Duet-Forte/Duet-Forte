using Mono.Cecil.Cil;
using UnityEngine;

// 클래스 설명 : 여러 메뉴를 등록하고, 키보드를 이용한 메뉴 조작 지원을 위한 클래스
public class MenuSelector : MonoBehaviour
{
    private Menu[] menuArray;
    private int currentIndex;
    private int previousIndex;

    public void InitSetting()
    {
        menuArray = GetComponentsInChildren<Menu>();

        for(int index = 0; index < menuArray.Length; ++index)
        {
            menuArray[index].InitSettings(this, index);
        }
        currentIndex = 0;
        previousIndex = 0;
    }

    private void Start()
    {
        InitSetting();
    }

    private void Update() //추후 input System 적용 예정.
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int changedIndex = currentIndex - 1;
            SetIndex(changedIndex);
            menuArray[currentIndex].OnSelected();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
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

    public void SetIndex(int index)
    {
        previousIndex = currentIndex;
        menuArray[previousIndex].OnDeselected();
        currentIndex = index;

        if (currentIndex >= menuArray.Length)
        {
            currentIndex = 0;
        }
        if (currentIndex < 0)
        {
            currentIndex = menuArray.Length - 1;
        }

        Debug.Log("인덱스 변경 " + currentIndex);
    }
}
