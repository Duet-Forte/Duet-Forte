using UnityEngine.InputSystem;
using UnityEngine;

// Ŭ���� ���� : ���� �޴��� ����ϰ�, Ű���带 �̿��� �޴� ���� ������ ���� Ŭ����
public class MenuSelector : MonoBehaviour
{
    [SerializeField] private Transform menuParent;
    protected Menu[] menuArray;
    protected int currentIndex;
    protected int previousIndex;
    protected InputAction uiControl;
    public Menu[] Menu {get => menuArray; }

    public virtual void InitSetting()
    {
        menuArray = GetComponentsInChildren<Menu>();
        if (menuArray == null ) 
        {
            menuArray = menuParent.GetComponentsInChildren<Menu>();
        }
        for (int index = 0; index < menuArray.Length; ++index)
        {
            menuArray[index].InitSettings(this, index);
        }
        currentIndex = 0;
        previousIndex = 0;
    }

    public virtual void SetIndex(int index)
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
    }
}
