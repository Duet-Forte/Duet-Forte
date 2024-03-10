using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> currentNoteList = new List<GameObject>();

    [SerializeField] Transform center = null;
    [SerializeField] RectTransform[] timingRect = null; //0: Perfect 1: Great 2: Good 3:Bad
    Vector2[] timingBoxs = null; //������ x���� Ÿ�̹� �ּڰ� y���� Ÿ�̹� �ִ�

    [SerializeField] private bool isOnBeat = false; // ��Ʈ�� ������ ���߾��� �������� true 

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
    /// �Է� Ű�� ������ üũ�ϴ� �Լ�
    /// ��ȯ�� ���� -1�� fail 0�� Perfect 1�� Great 2�� Good 3�� Bad
    /// -2�� �� ���� �� / -2�� PlayerAttackŬ�������� �ο���.
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
        return -1;//fail����
    }
    // Update is called once per frame
    void Update()
    {
        //�ӽ÷� ���� Ű�Է�
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
