using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class FlavorTextUI : MonoBehaviour
{
    [SerializeField] Sprite enemyPortrait;
    [SerializeField] TMP_Text name;
    [SerializeField] TMP_Text enemyInfo;
    [SerializeField] private float defaultX;
    [SerializeField] private float popX;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    
    public void InitSetting(Sprite enemyPortrait, string name, string enemyInfo) 
    {
        GetComponent<ScreenSpaceCameraUI>().InitSettings();
        this.enemyPortrait = enemyPortrait;
        this.name.text = name;
        this.enemyInfo.text = enemyInfo;
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine( AppearAndDisappear());
        

    }
    IEnumerator AppearAndDisappear() 
    {
        rectTransform.DOAnchorPosX(popX, 0.2f);
        yield return new WaitForSeconds(6.5f);
        rectTransform.DOAnchorPosX(defaultX, 0.2f).OnComplete(()=>Destroy(transform.parent.gameObject));
    }
    // Update is called once per frame
    
}
