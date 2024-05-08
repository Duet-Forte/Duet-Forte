using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject layOut;
    [Header("ȭ�� ������Ʈ")]
    [Tooltip("�̹������ϰ� Text_TMP�� �ڽ� ������Ʈ�� ���� �־�� ��.")]
    [SerializeField] private GameObject windowPrefab;
    [Header("ȭ�� ����")]
    [Tooltip("�� �������� �ִ� 3���� ��µ�.")]
    [SerializeField] private int windowCount;
    [SerializeField] private List<string> describes;
    [SerializeField] private List<Sprite> screenshots;
    GameObject[] windows;
    private bool isTutorial=true;
    private const int minPage=0;
    private int maxPage;
    private int restWindow;
    private const int MAX_ACTIVE_WINDOW_COUNT = 3;
    private int currentPage = 0; //0으로 시작

    private void Start()
    {
        InitSettings();
    }
    public void InitSettings() {
        windows= new GameObject[windowCount];
        maxPage = windowCount / MAX_ACTIVE_WINDOW_COUNT;
        if (windowCount % MAX_ACTIVE_WINDOW_COUNT > 0)//잔여 window가 있으면 마지막 페이지 추가
        {
            maxPage++;
        }
        for(int i=0; i<windowCount;i++) {
            windows[i] = windowPrefab;
            windows[i].GetComponentInChildren<TMP_Text>().text=describes[i];
            windows[i].GetComponentInChildren<Image>().sprite=screenshots[i];
        }
        Appear(currentPage);
    }

    public void Appear(int currentPage) {
        int startIndex = currentPage * MAX_ACTIVE_WINDOW_COUNT;
        if (layOut.transform.childCount != 0)
        {
            foreach (Transform child in layOut.transform)
            {
                Destroy(child.gameObject);
            }
        }

        for (int currentIndex = currentPage * MAX_ACTIVE_WINDOW_COUNT; currentIndex < startIndex + MAX_ACTIVE_WINDOW_COUNT; currentIndex++) {
            Debug.Log(currentIndex);
            GameObject tmp= Instantiate(windows[currentIndex],Vector3.zero,Quaternion.identity);
            tmp.transform.SetParent(layOut.transform);
        }

        StartCoroutine(WaitingForInput());


    }
    private IEnumerator WaitingForInput() {
        while (isTutorial) {
            if (Input.GetKeyDown(KeyCode.F)) { //Prev
                currentPage--;
                if(currentPage < 0)currentPage = 0;
                Debug.Log("pressed F");
                Appear(currentPage);
                
            }
            if (Input.GetKeyDown(KeyCode.J)) { //Next
                currentPage++;
                if (currentPage > maxPage)currentPage = maxPage;
                Debug.Log("pressed J");
                Appear(currentPage);
                
            }
            if(currentPage==maxPage&&Input.GetKeyDown(KeyCode.Escape))// out of tutorial
            yield return null;
        }
        
        }

    public void DisAppear(int currentPage){

    }
}
