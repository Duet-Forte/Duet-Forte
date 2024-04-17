using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Util;

public class DamageUI : MonoBehaviour
{
    private Image[] image=new Image[11];
    private Queue<Sprite> BlueDamagequeue;
    private Queue<Sprite> RedDamagequeue;
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
        for (int i = 0; i < 11; i++){
            image[i]=transform.Find("Damage"+i).GetComponent<Image>();
        }
        rectTransform = GetComponent<RectTransform>();
        RedDamagequeue = new Queue<Sprite>();
        BlueDamagequeue = new Queue<Sprite>();
        transform.SetParent(canvas.transform);
        this.pool = pool;
        SetLayout();
        ResetSettings();
    }

    public void ParseDamage(Damage damage)
    {
        int calculatedDamage = damage.GetCalculatedDamage();
        int digit = (int)Mathf.Log10(calculatedDamage)+1;
        int index = 0;
        List<int> damageByDigit= new List<int>();
        if (damage.GetDamageType()==CustomEnum.DamageType.Slash)
        {
            Debug.Log("RedDamage");
            while (calculatedDamage> 0)
            {

                int tmp = calculatedDamage % 10;
                damageByDigit.Add(tmp);
                calculatedDamage /= 10;
                
            }
            damageByDigit=Enumerable.Reverse<int>(damageByDigit).ToList();
            foreach (int i in damageByDigit) {
                RedDamagequeue.Enqueue(DamageUIContainer.RedSkins[i]);
            }
            rectTransform.sizeDelta = new Vector2(Const.DAMAGEUI_UI_WIDTH*RedDamagequeue.Count, Const.DAMAGEUI_UI_HEIGHT);


            while (RedDamagequeue.Count > 0)
            {
                image[index].gameObject.SetActive(true);
                image[index++].sprite = RedDamagequeue.Dequeue();
            }
        }
        else if(damage.GetDamageType()==CustomEnum.DamageType.Pierce)
        {
            Debug.Log("BlueDamage");
            while (calculatedDamage > 0)
            {

                int tmp = calculatedDamage % 10;
                damageByDigit.Add(tmp);
                calculatedDamage /= 10;

            }
            damageByDigit = Enumerable.Reverse<int>(damageByDigit).ToList();
            foreach (int i in damageByDigit)
            {
                BlueDamagequeue.Enqueue(DamageUIContainer.BlueSkins[i]);
            }
            rectTransform.sizeDelta = new Vector2(Const.DAMAGEUI_UI_WIDTH * BlueDamagequeue.Count, Const.DAMAGEUI_UI_HEIGHT);

            while (BlueDamagequeue.Count > 0)
            {
                image[index].gameObject.SetActive(true);
                image[index++].sprite = BlueDamagequeue.Dequeue();
            }
        }

        /*while (damage > 0)
        {
            RedDamagequeue.Enqueue(DamageUIContainer.Skins[damage % 10]);
            damage /= 10;
        }*/
        
        //rectTransform.sizeDelta = new Vector2(Const.DAMAGEUI_UI_WIDTH * RedDamagequeue.Count, Const.DAMAGEUI_UI_HEIGHT);

        

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
