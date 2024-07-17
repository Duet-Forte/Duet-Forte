using Util;
using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.Pool;

public class SkillSetter : ScrollPool
{
    [Header("Skill Setter Settings")]
    [Space]
    [SerializeField] private SkillSaver skillSaver;
    [Header("Debug")]
    [SerializeField] private int currentMinIndex;
    [SerializeField] private int currentMaxIndex;
    private SkillSelector skillSelector;
    private bool isSkillSelectorSelected; // 현재 SkillSaver와 SkillSelector 중 어디에 위치해 있는지 적용.
    public ObjectPool<ScrollContent> CurrentPool { get => contentPool; }
    public SkillSelector SkillSelector { get => skillSelector; }
    public SkillSaver SkillSaver { get => skillSaver; }
    public bool IsSelectorSelected { get => isSkillSelectorSelected; set => isSkillSelectorSelected = value; }

    private void Start()
    {   
        InitSettings();
    }
    public void InitSettings()
    {
        BindEvent();
        InitSettings((DataBase.Skill.Data.Length / Const.CONTENT_IN_ROW) + 1);
        currentMinIndex = 0;
        currentMaxIndex = createStandard - 1;
        for (int i = 0; i < createStandard; ++i)
        {
            contentPool.Get();
            pooledContents.Last.Value.RefreshContent(i);
        }
        isSkillSelectorSelected = true;
        skillSelector = transform.GetOrAddComponent<SkillSelector>();
        skillSelector.InitSettings(this);
        skillSaver.InitSettings(this);
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
        if (currentMaxIndex > DataBase.Skill.Data.Length / Const.CONTENT_IN_ROW)
            return;
        currentMaxIndex++;
        currentMinIndex++;
        pooledContents.First.Value.RefreshContent(currentMaxIndex);
    }
}
