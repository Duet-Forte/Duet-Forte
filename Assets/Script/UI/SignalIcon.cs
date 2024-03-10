using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SignalIcon : MonoBehaviour
{
    Sequence popEnable;
    RectTransform rectTransform; 
    private void Start()
    {
        rectTransform= GetComponent<RectTransform>();
        popEnable = DOTween.Sequence().SetAutoKill(false).OnStart(()=> {//공격 및 방어 아이콘 활성화 애니메이션
            transform.localScale = new Vector3(2f, 2f);
            Color tempColor = GetComponent<Image>().color;
            tempColor.a = 0;
        }).Append(gameObject.transform.DOScale(new Vector3(1f,1f), 0.1f)).SetEase(Ease.InQuad)
        .Join(GetComponent<Image>().DOFade(1f,0.1f));
    }

    public void AttackEffect() {
        
        rectTransform.DOShakePosition(0.3f, 0.5f, 100, 30);
    }
    private void OnEnable()
    {
        popEnable.Restart();
    }
}
