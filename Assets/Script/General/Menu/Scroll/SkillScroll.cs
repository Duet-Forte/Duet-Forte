using Util;
using UnityEngine;
using Unity.VisualScripting;

public class SkillScroll : ScrollPool
{
    [Header("Skill Scroll Settings")]
    [Space]
    [SerializeField] private PlayerSkill[] data;
    [SerializeField] private int currentMinIndex;
    [SerializeField] private int currentMaxIndex;

    private void Start()
    {
        InitSettings();
    }
    public void InitSettings()
    {
        BindEvent();
        InitSettings((data.Length / Const.CONTENT_IN_ROW) + 1);
        currentMinIndex = 0;
        currentMaxIndex = createStandard - 1;
        for (int i = 0; i < createStandard; ++i)
        {
            contentPool.Get();
            pooledContents.Last.Value.RefreshContent(i);
        }
    }

    public override ScrollContent InitScrollContent(GameObject gameObject)
    {
        SkillContent skillContent = gameObject.GetComponent<SkillContent>();
        if (skillContent == null)
            skillContent = gameObject.AddComponent<SkillContent>();
        skillContent.InitSettings(data, contentPool);
        return skillContent;
    }

    private void BindEvent()
    {
        onDown -= IncreaseRowIndex;
        onDown += IncreaseRowIndex;
        onUp -= DecreaseRowIndex;
        onUp += DecreaseRowIndex;
    }

    private void DecreaseRowIndex()
    {
        if (currentMinIndex - 1 < 0)
            return;
        currentMinIndex--;
        currentMaxIndex--;
        pooledContents.Last.Value.RefreshContent(currentMinIndex);
    }

    private void IncreaseRowIndex()
    {
        if (currentMaxIndex > data.Length / Const.CONTENT_IN_ROW)
            return;
        currentMaxIndex++;
        currentMinIndex++;
        pooledContents.First.Value.RefreshContent(currentMaxIndex);
    }
}
