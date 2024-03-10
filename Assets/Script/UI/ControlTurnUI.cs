using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class ControlTurnUI : MonoBehaviour
{
    Image blackBox;
    TMP_Text turnText;
    Vector2 originBlackBoxScale = new Vector2(1, 3.3f);
    Vector2 appearanceBlackBoxScale = new Vector2(1, 1);
    Vector2 disappearanceBlackBoxScale = new Vector2(1, 1.2f);
    int blackBoxAlphaValue = 114;

    string turn = "TURN";
    
    public void InitSettings() {
        blackBox = GetComponentInChildren<Image>();
        turnText = GetComponentInChildren<TMP_Text>();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="turnCount">���� �� ��° ������</param>
    public void AppearanceTurnUI(int turnCount) {
        RestartSetting();
        turnText.text = turn + " " + turnCount;//�� ����
        turnText.DOFade(1, 0.2f);//�ؽ�Ʈ ��Ÿ��
        blackBox.gameObject.transform.DOScale(appearanceBlackBoxScale, 0.2f);//�ڽ� ��Ÿ��
        blackBox.DOFade(0.65f, 0.2f);//���� ���� 0���� 0.4
        Invoke("DisappearanceTurnUI",1.5f);//1.5�ʰ� ������ �� �����

    }
    public void DisappearanceTurnUI() {
        blackBox.gameObject.transform.DOScale(disappearanceBlackBoxScale, 0.1f);
        blackBox.DOFade(0, 0.1f);
        turnText.DOFade(0, 0.1f);
    }
    void RestartSetting() {
        turnText.text = "";
        blackBox.color= new Color(0, 0, 0, 0);
        blackBox.transform.localScale = originBlackBoxScale;
    }


}
