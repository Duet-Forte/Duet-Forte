using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Util;

public class DamageUI : MonoBehaviour
{
    [SerializeField] private Image[] effectAndJudges = new Image[2];
    private Image[] image=new Image[11];
    private Queue<Sprite> BlueDamagequeue;
    private Queue<Sprite> RedDamagequeue;
    private RectTransform rectTransform;
    private RectTransform canvasRectTransform;
    private IObjectPool<DamageUI> pool;
    private int YMAX = 15;
    private Image judgeEffect;
    private Image judgeLetter;
    private Sprite[] judgeEffects;
    private Sprite[] judgeLetters;
    private ScreenSpaceCameraUI cameraUI;

    public RectTransform Position { get 
        { if (canvasRectTransform == null)
                canvasRectTransform = GetComponentInParent<RectTransform>();
            return canvasRectTransform; } 
    }
    public void InitSettings(IObjectPool<DamageUI> pool)
    {
        cameraUI = transform.GetComponent<ScreenSpaceCameraUI>();
        if (cameraUI == null)
            cameraUI = gameObject.AddComponent<ScreenSpaceCameraUI>();
        GameObject damagePool= transform.Find("Damage").gameObject;
        for (int i = 0; i < 11; i++){
            image[i]=damagePool.transform.Find("Damage"+i).gameObject.GetComponent<Image>();
        }
        judgeEffect = image[0].transform.Find("DamageJudgeEffect").gameObject.GetComponent<Image>();
        judgeLetter = image[0].transform.Find("DamageJudgeLetter").gameObject.GetComponent<Image>();

        judgeEffects = Resources.LoadAll<Sprite>("UI/Damage/Effect");
        judgeLetters = Resources.LoadAll<Sprite>("UI/Damage/Letter");

        rectTransform = transform.Find("Damage").gameObject.GetComponent<RectTransform>();
        GetComponentInParent<Canvas>().worldCamera = Camera.main;
        canvasRectTransform = GetComponentInParent<RectTransform>();
        RedDamagequeue = new Queue<Sprite>();
        BlueDamagequeue = new Queue<Sprite>();
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

        judgeEffect.sprite = judgeEffects[Mathf.Min((int)(damage.JudgeName) - 1, judgeEffects.Length - 1)];
        judgeLetter.sprite = judgeLetters[Mathf.Min((int)(damage.JudgeName) - 1, judgeEffects.Length - 1)];

        if (damage.GetDamageType() == Util.CustomEnum.DamageType.Slash)
        {
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
        else if(damage.GetDamageType()==Util.CustomEnum.DamageType.Pierce)
        {
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
        HorizontalLayoutGroup horizontalLayoutGroup = transform.Find("Damage").GetComponent<HorizontalLayoutGroup>();
        if (horizontalLayoutGroup == null)
            horizontalLayoutGroup = transform.Find("Damage").gameObject.AddComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.enabled = true;
        horizontalLayoutGroup.spacing = 5;
        horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
        horizontalLayoutGroup.childControlHeight = true;
        horizontalLayoutGroup.childControlWidth = true;
    }

    public void MoveUI()
    {
        StartCoroutine(FadeSwitch(true));
       
        
        float randomX=Random.Range(0f, 1f);
        float randomY=Random.Range(0f, 1f);

        canvasRectTransform.DOMove(new Vector2(gameObject.transform.position.x+8f,gameObject.transform.position.y+4f), Const.DAMAGEUI_FADE_SPEED)
            .SetEase(Ease.Linear)
            .OnComplete(() => pool.Release(this));

        StartCoroutine(FadeSwitch(false));
    }


    private IEnumerator FadeSwitch(bool onOff) {
        if (onOff) {
            foreach (Image i in effectAndJudges)
            {
                i.color = new Color(1, 1, 1, 1);
            }
            foreach (Image i in image)
            {
                i.color = new Color(1, 1, 1, 1);
            }

        }
        if (!onOff) {
            float delay = 0.6f;
            yield return new WaitForSeconds(delay);
            foreach (Image i in effectAndJudges)
            {
                i.DOFade(0f, Const.DAMAGEUI_FADE_SPEED- delay);
            }
            foreach (Image i in image)
            {
                i.DOFade(0f, Const.DAMAGEUI_FADE_SPEED- delay);
            }

        }
    
    }
}
