using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> currentNoteList = new List<GameObject>();

    [SerializeField] Transform center = null;
    [SerializeField] RectTransform[] timingRect = null; //0: Perfect 1: Great 2: Good 3:Bad
    Vector2[] timingBoxs = null; //벡터의 x값은 타이밍 최솟값 y값은 타이밍 최댓값

    [SerializeField] private bool isOnBeat = false; // 노트가 판정선 정중앙을 지날때만 true 

    public bool IsOnBeat { get => isOnBeat; }
    // Start is called before the first frame update
    void Start()
    {
        timingBoxs = new Vector2[timingRect.Length];

        for (int i = 0; i < timingRect.Length; i++) {
            timingBoxs[i].Set(center.localPosition.x - timingRect[i].rect.width / 2,
                              center.localPosition.x + timingRect[i].rect.width / 2);
        }

        
    }
    /// <summary>
    /// 입력 키의 판정을 체크하는 함수
    /// 반환된 값이 -1은 fail 0은 Perfect 1은 Great 2는 Good 3은 Bad
    /// -2는 한 박자 쉼 / -2는 PlayerAttack클래스에서 부여함.
    /// </summary>
    /// 
    public int CheckTiming() {

        for (int i = 0; i < currentNoteList.Count; i++) {
            float t_notePosX = currentNoteList[i].transform.localPosition.x;

            for (int x = 0; x < timingBoxs.Length; x++) {


                if (timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y) {

                    Debug.Log("Hit " + timingRect[x].name) ;
                    return x;
                }
            }
        
        }
        return -1;//fail판정
    }
    // Update is called once per frame
    void Update()
    {
        //임시로 만든 키입력
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckTiming();
        }

        /*if (currentNoteList[0].transform.localPosition.x >= timingBoxs[0].x && currentNoteList[0].transform.localPosition.x <= timingBoxs[0].y)
        {

            isOnBeat = true;
        }
        else { isOnBeat = false; }*/

        for (int i = 0; i < currentNoteList.Count; i++) {

            if (currentNoteList[i].transform.localPosition.x >= timingBoxs[0].x && currentNoteList[i].transform.localPosition.x <= timingBoxs[0].y)
            {
                if (!isOnBeat) { 
                isOnBeat = true;
                    break;
                }
                
            }
            else { isOnBeat = false; }
        }
        
    }
}
