using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // 비활성화 전용 불리언값.
    // 예를 들어 LoadGame에서 세이브 데이터가 없으면 기능을 비활성화.
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
        // 마우스를 통해 메뉴를 선택하면 이전 선택 메뉴가
        // 선택된 것이 갱신되지 않는 이슈를 해결하기 위해.
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
