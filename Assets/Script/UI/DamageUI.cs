using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Util;

public class DamageUI : MonoBehaviour
{
    private Image[] image;
    private Queue<Sprite> queue;
    private RectTransform rectTransform;
    private IObjectPool<DamageUI> pool;
    private int YMAX = Screen.height;


    public RectTransform RectTransform { get 
        { if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
          return rectTransform; } 
    }
    public void InitSettings(IObjectPool<DamageUI> pool, Canvas canvas)
    {
        image = GetComponentsInChildren<Image>();
        rectTransform = GetComponent<RectTransform>();
        queue = new Queue<Sprite>();
        transform.SetParent(canvas.transform);
        this.pool = pool;
        SetLayout();
        ResetSettings();
    }

    public void ParseDamage(int damage)
    {
        while (damage > 0)
        {
            queue.Enqueue(DamageUIContainer.Skins[damage % 10]);
            damage /= 10;
        }

        rectTransform.sizeDelta = new Vector2(Const.DAMAGEUI_UI_WIDTH * queue.Count, Const.DAMAGEUI_UI_HEIGHT);

        int index = 0;
        while (queue.Count > 0)
        {
            image[index].gameObject.SetActive(true);
            image[index++].sprite = queue.Dequeue();
        }

        MoveUI();
    }
    public void ResetSettings()
    {
        foreach(Image damageImage in image)
        {
            damageImage.gameObject.SetActive(false);
        }
       
    }

    private void SetLayout()
    {
        HorizontalLayoutGroup horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        if (horizontalLayoutGroup == null)
            horizontalLayoutGroup = transform.AddComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.enabled = true;
        horizontalLayoutGroup.spacing = 5;
        horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
        horizontalLayoutGroup.childControlHeight = true;
        horizontalLayoutGroup.childControlWidth = true;
    }

    private void MoveUI()
    {
        rectTransform.DOMoveY(YMAX, Const.DAMAGEUI_FADE_SPEED)
            .SetEase(Ease.InElastic)
            .OnComplete(() => pool.Release(this));
    }
}
