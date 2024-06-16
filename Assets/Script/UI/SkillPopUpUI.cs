using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class SkillPopUpUI : MonoBehaviour
{
    [SerializeField] Sprite dummySprite;
    [SerializeField] string dummyText;
    [SerializeField] private int moveOffset;
    public void Appear(Sprite skillImage,string skillName) {

        transform.GetChild(1).GetComponent<Image>().sprite=skillImage;
        transform.GetChild(0).GetComponent<TMP_Text>().text=skillName;
        Debug.Log($"skillPopUpUIÀÇ x°ª : {transform.position.x}");
        transform.DOMove(new Vector2(transform.position.x + moveOffset, transform.position.y), 0.2f);

    }
    public void Disappear() {

        transform.DOMove(new Vector2(transform.position.x - moveOffset, transform.position.y), 0.2f);
    }
}
