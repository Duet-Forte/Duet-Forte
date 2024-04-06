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

    // Start is called before the first frame update
    
    public void InitSetting(Sprite enemyPortrait, string name, string enemyInfo) {
        this.enemyPortrait = enemyPortrait;
        this.name.text = name;
        this.enemyInfo.text = enemyInfo;
        StartCoroutine( AppearAndDisappear());
        

    }
    IEnumerator AppearAndDisappear() {
        transform.DOMove(new Vector2(transform.position.x - 400, transform.position.y), 0.4f);
        yield return new WaitForSeconds(6.5f);
        transform.DOMove(new Vector2(transform.position.x + 400, transform.position.y), 0.4f).OnComplete(()=>Destroy(transform.parent.gameObject));
    }
    // Update is called once per frame
    
}
