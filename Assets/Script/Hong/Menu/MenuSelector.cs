using Mono.Cecil.Cil;
using UnityEngine;

// Ŭ���� ���� : ���� �޴��� ����ϰ�, Ű���带 �̿��� �޴� ���� ������ ���� Ŭ����
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

    private void Update() //���� input System ���� ����.
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

        Debug.Log("�ε��� ���� " + currentIndex);
    }
}
