using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // ��Ȱ��ȭ ���� �Ҹ���.
    // ���� ��� LoadGame���� ���̺� �����Ͱ� ������ ����� ��Ȱ��ȭ.
    protected bool isEnabled;

    protected MenuSelector selector;
    protected Image image;
    protected int index;

    public bool isEnableMouseControl;

    public virtual void InitSettings(MenuSelector selector, int index)
    {
        if(this.selector == null)
            this.selector = selector;
        this.index = index;
        image = GetComponent<Image>();
        isEnabled = true;
    }
    public virtual void OnSelected()
    {

    }

    public virtual void OnDeselected()
    {

    }

    public virtual void OnPointerEnter(PointerEventData data)
    {
        // ���콺�� ���� �޴��� �����ϸ� ���� ���� �޴���
        // ���õ� ���� ���ŵ��� �ʴ� �̽��� �ذ��ϱ� ����.
        if(isEnableMouseControl)
        {
            selector.SetIndex(index);
            OnSelected();
        }
    }

    public virtual void OnPointerExit(PointerEventData data)
    {
        if(isEnableMouseControl) 
            OnDeselected();
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (isEnableMouseControl)
            OnPressed();
    }

    public abstract void OnPressed();
}
