using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class FlavorTextUI : MonoBehaviour
{
    [SerializeField] Image enemyPortrait;
    [SerializeField] TMP_Text enemyName;
    [SerializeField] TMP_Text enemyInfo;
    [SerializeField] private float defaultX;
    [SerializeField] private float popX;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    
    public void InitSetting(Sprite enemyPortrait, string enemyName, string enemyInfo) 
    {
        GetComponent<ScreenSpaceCameraUI>().InitSettings();
        this.enemyPortrait.sprite = enemyPortrait;
        this.enemyName.text = enemyName;
        Debug.Log(enemyInfo);
        enemyInfo.Replace("\\n", "\n");
        this.enemyInfo.text = enemyInfo;
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine( AppearAndDisappear());
        

    }
    IEnumerator AppearAndDisappear() 
    {
        yield return new WaitForSeconds(1.5f);
        rectTransform.DOAnchorPosX(700, 0.2f);//팝업
        yield return new WaitForSeconds(6.5f);
        rectTransform.DOAnchorPosX(defaultX, 0.2f).OnComplete(()=>Destroy(transform.parent.gameObject));//원래 자리
    }
    
    
}
