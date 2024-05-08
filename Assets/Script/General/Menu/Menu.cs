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
        AkSoundEngine.PostEvent("MainMenu_Hover_SFX", gameObject);
    }

    public virtual void OnDeselected()
    {

    }

    public virtual void OnPointerEnter(PointerEventData data)
    {
        // ���콺�� ���� �޴��� �����ϸ� ���� ���� �޴���
        // ���õ� ���� ���ŵ��� �ʴ� �̽��� �ذ��ϱ� ����.
        selector.SetIndex(index);
        OnSelected();
    }

    public virtual void OnPointerExit(PointerEventData data)
    {
        OnDeselected();
    }

    public void OnPointerClick(PointerEventData data)
    {
        OnPressed();
    }

    public abstract void OnPressed();
}
