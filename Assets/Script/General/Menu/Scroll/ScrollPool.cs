using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

/// <summary>
/// Add to scroll view.
/// </summary>
public abstract class ScrollPool : MonoBehaviour
{
    [SerializeField] private GameObject contentPrefab;
    [SerializeField] private float padding;
    [SerializeField] private float spacing;
    [SerializeField] private float resetThreshold;
    [SerializeField] private int poolSize;
    [SerializeField] private RectTransform contentArea;

    private ObjectPool<ScrollContent> contentPool;
    private LinkedList<ScrollContent> pooledContents;

    private LinkedList<float> contentYPositions;
    private bool isDown;
    private float contentHeight;
    private float currentYStandard;
    private int downCount;
    private float previousYPos;

    private int testint = 0;
    private const int DEFAULT_Y_POSITION = 0;
    public void InitSettings<T>(T[] contents) where T : class
    {
        //Initializing factors
        int numberOfContents = contents.Length;
        contentHeight = contentPrefab.GetComponent<RectTransform>().rect.height;
        pooledContents = new LinkedList<ScrollContent>();
        contentYPositions = new LinkedList<float>();
        float standardYPositionsValue = spacing + (contentHeight * 0.5f);
        contentYPositions.AddLast(standardYPositionsValue);
        previousYPos = DEFAULT_Y_POSITION;
        isDown = true;
        contentPool = new ObjectPool<ScrollContent>(CreateObject, OnGet, OnRelease, OnDestroyPool, maxSize: poolSize);

        //Set Size
        SetAreaSize(numberOfContents);

        for (int i = 0; i < poolSize; ++i)
            contentPool.Get();
    }

    /// <summary>
    /// Set Size of Scroll View. It is proportional to the size of the content.
    /// </summary>
    /// <param name="numberOfContents"></param>
    public void SetAreaSize(int numberOfContents)
    {
        contentArea.sizeDelta = new Vector2(contentArea.sizeDelta.x, 
            (spacing * 2) + (padding * (numberOfContents - 1) +
            (contentPrefab.GetComponent<RectTransform>().rect.height * numberOfContents)));
    }

    /// <summary>
    /// It is called when contentMenu is spawned.
    /// </summary>
    /// <param name="scrollContent"></param>
    public void SetPosition(ScrollContent scrollContent)
    {
        RectTransform rect = scrollContent.GetComponent<RectTransform>();
        float currentY;
        if(isDown)
        {
            // Check whether the scroll is over the range of scroll view.
            if (Mathf.Abs(contentYPositions.Last.Value - padding - contentHeight) > contentArea.rect.height)
                return;
            currentY = contentYPositions.AddLast(contentYPositions.Last.Value - padding - contentHeight).Value;
            if (contentYPositions.First.Value > 0)
                contentYPositions.RemoveFirst();
        }
        else
        {
            // Check whether the scroll is over the range of scroll view.
            if (contentYPositions.First.Value + padding + contentHeight >= 0)
                return;
            currentY = contentYPositions.AddFirst(contentYPositions.First.Value + padding + contentHeight).Value;
        }
        rect.localPosition = new Vector3(rect.localPosition.x, currentY, rect.localPosition.z);
        Debug.Log(contentYPositions.Count);
    }

    public void Update()
    {
        //Check current scroll state. If the scroll moves beyond a certain level, contents menu will move.
        isDown = previousYPos >= contentArea.localPosition.y ? false : true; 
        currentYStandard = contentArea.localPosition.y - (resetThreshold * downCount);
        // Check whether the scroll is over the range of scroll view.
        if (contentArea.localPosition.y < 0)
            return;
        if (currentYStandard >= resetThreshold)
        {
            // Check whether the scroll is over the range of scroll view.
            if (Mathf.Abs(contentYPositions.Last.Value - padding - contentHeight) > contentArea.rect.height)
                return;
            downCount++;
            pooledContents.First.Value.ReleaseSelf();
            contentPool.Get();
        }
        else if (currentYStandard < 0)
        {
            downCount--;
            pooledContents.Last.Value.ReleaseSelf();
            contentPool.Get();
        }
        previousYPos = contentArea.localPosition.y ;
    }

    //public abstract void SetContents<T>(T[] contents) where T : class;

    #region pool Funcs
    protected ScrollContent CreateObject()
    {
        GameObject go = Instantiate(contentPrefab, contentArea);
        go.name = $"Skill {testint++}";
        ScrollContent scrollContent = go.GetComponent<ScrollContent>();
        if (scrollContent == null)
            scrollContent = go.AddComponent<ScrollContent>();
        scrollContent.InitSetting(contentPool);
        pooledContents.AddLast(scrollContent);
        return scrollContent;
    }
    protected void OnGet(ScrollContent scrollContent)
    {
        SetPosition(scrollContent);
        scrollContent.gameObject.SetActive(true);
    }

    protected void OnRelease(ScrollContent scrollContent)
    {
        if(isDown)
        {
            contentYPositions.RemoveFirst();
            pooledContents.AddLast(pooledContents.First.Value);
            pooledContents.RemoveFirst();
        }
        else
        {
            contentYPositions.RemoveLast();
            pooledContents.AddFirst(pooledContents.Last.Value);
            pooledContents.RemoveLast();
        }
        Debug.Log(scrollContent);
    }

    protected void OnDestroyPool(ScrollContent gameObject)
    {

    }
    #endregion
}
