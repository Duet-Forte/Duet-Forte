using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Util.CustomEnum;

public class SkillGainWindow : MonoBehaviour
{
    [SerializeField] private PlayerSkill skill;
    [SerializeField] private Image frame, skillIcon;
    [SerializeField] private RectTransform arrow;
    [SerializeField] private TextMeshProUGUI skillName, skillDescription;
    [SerializeField] private Transform command;
    [SerializeField] private GameObject commandLetterPrefab;
    [SerializeField] private Sprite jSprite, fSprite, blueFrame, orangeFrame;
    [SerializeField] private RectTransform rect;
    private int standard;
    public async UniTask InitSettings(PlayerSkill skill)
    {
        skillIcon.sprite = skill.SkillIcon;
        skillName.text = skill.SkillName;
        skillDescription.text = skill.Information;
        standard = 0;
        for (int i = 0; i < skill.SkillCommand.Length; ++i)
        {
            Image temp = Instantiate(commandLetterPrefab, command).GetComponent<Image>();
            if (skill.SkillCommand[i] == "A")
            {
                temp.sprite = fSprite;
                standard++;
            }
            else if (skill.SkillCommand[i] == "B")
            {
                temp.sprite = jSprite;
                standard--;
            }
        }
        
        if(standard >= 0)
            frame.sprite = orangeFrame;
        else
            frame.sprite = blueFrame;

        gameObject.SetActive(true);
        arrow.DOAnchorPosY(arrow.anchoredPosition.y + 20, 1)
           .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);

        rect.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        rect.DOScale(Vector3.one, 0.2f);
        await UniTask.WaitUntil(() => SceneManager.Instance.InputController.IsKeyTriggered(PlayerAction.Interact));
        Tween tween = DOTween.Sequence()
            .Append(rect.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f))
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
