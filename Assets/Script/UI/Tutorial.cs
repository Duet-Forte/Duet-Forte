using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject layOut;
    [SerializeField] private GameObject windowPrefab;
    [SerializeField] private int windowCount;
    [SerializeField] private List<string> describes;
    [SerializeField] private List<Sprite> screenshots;
    [SerializeField] private string[] titles;
    [SerializeField] private TMP_Text title;
    GameObject[] windows;
    private bool isTutorial=true;
    private int maxPage;
    private const int MAX_ACTIVE_WINDOW_COUNT = 3;
    private int currentPage = 0; //0으로 시작

    private void Start()
    {
       StartCoroutine(InitSettings());
    }
    public IEnumerator InitSettings() {
        windows= new GameObject[windowCount];
        maxPage = (windowCount / MAX_ACTIVE_WINDOW_COUNT)-1;
        if (windowCount % MAX_ACTIVE_WINDOW_COUNT > 0)//잔여 window가 있으면 마지막 페이지 추가
        {
            maxPage++;
        }

        for(int i=0; i<windowCount;i++) {
            windows[i] = Instantiate(windowPrefab);
            windows[i].GetComponentInChildren<TMP_Text>().text=describes[i];
            windows[i].transform.Find("Screenshot").GetComponent<Image>().sprite=screenshots[i];
        }
        
        Appear(currentPage);
        yield return StartCoroutine(WaitingForInput());
    }

    public void Appear(int currentPage) {
        title.text = titles[currentPage];
        int startIndex = currentPage * MAX_ACTIVE_WINDOW_COUNT;
        if (layOut.transform.childCount != 0)
        {
            foreach (Transform child in layOut.transform)
            {
                Destroy(child.gameObject);
            }
        }

        for (int currentIndex = currentPage * MAX_ACTIVE_WINDOW_COUNT; currentIndex < startIndex + MAX_ACTIVE_WINDOW_COUNT; currentIndex++) {
            try
            {
                GameObject tmp = Instantiate(windows[currentIndex], Vector3.zero, Quaternion.identity);
                tmp.transform.SetParent(layOut.transform);
            }
            catch {
            
            }
            
        }
        
        


    }
    private IEnumerator WaitingForInput() {
        while (isTutorial) {
            if (Input.GetKeyDown(KeyCode.F)) { //Prev
                currentPage--;
                if(currentPage < 0)currentPage = 0;
                Appear(currentPage);
                
            }
            if (Input.GetKeyDown(KeyCode.J)) { //Next
                currentPage++;
                if (currentPage > maxPage)currentPage = maxPage;
                Appear(currentPage);
                
            }
            if (currentPage == maxPage && Input.GetKeyDown(KeyCode.Escape))
            { // out of tutorial
                isTutorial=false;
            }
            yield return null;

        }
        DisAppear();
        }

    private void DisAppear(){
        Destroy(gameObject);
    }
}
