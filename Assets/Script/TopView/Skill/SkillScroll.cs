using Util;
using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.Pool;

public class SkillScroll : ScrollPool
{
    [Header("Skill Scroll Settings")]
    [Space]
    [SerializeField] private SkillSaver skillSaver;
    [Header("Debug")]
    [SerializeField] private int currentMinIndex;
    [SerializeField] private int currentMaxIndex;
    private SkillSelector skillSelector;
    public ObjectPool<ScrollContent> CurrentPool { get => contentPool; }
    public SkillSelector SkillSelector { get => skillSelector; }

    private void Start()
    {
        InitSettings();
    }
    public void InitSettings()
    {
        BindEvent();
        InitSettings((DataBase.Instance.Skill.Data.Length / Const.CONTENT_IN_ROW) + 1);
        skillSelector = transform.GetOrAddComponent<SkillSelector>();
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
        skillContent.InitSettings(this, skillSaver);
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
        if (currentMaxIndex > DataBase.Instance.Skill.Data.Length / Const.CONTENT_IN_ROW)
            return;
        currentMaxIndex++;
        currentMinIndex++;
        pooledContents.First.Value.RefreshContent(currentMaxIndex);
    }
}
