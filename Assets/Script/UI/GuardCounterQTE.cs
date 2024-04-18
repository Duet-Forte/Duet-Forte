using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Util.CustomEnum;
public class GuardCounterQTE : MonoBehaviour
{
    [SerializeField]GameObject outCircle;
    [SerializeField]GameObject inCircle;
    JudgeName QTEJudge;
    Vector3 originOutCircleScale=new Vector3(4,4,4);
    float secondPerBeat;
    Vector2 perfectZone=new Vector2(0.95f,1.15f);
    Vector2 greatZone = new Vector2(1.15f, 1.4f);
    Vector2 goodZone = new Vector2(1.4f, 1.8f);
    Vector2 missZone = new Vector2(1.8f, 4f);

    public JudgeName GetQTEJudge { get => QTEJudge; }
    private void OnEnable()
    {
        
        secondPerBeat = (float)Metronome.instance.SecondsPerBeat;   
        
    }

    public IEnumerator StartQTE(Vector2 spawnPos)
    {
        outCircle.transform.position = spawnPos;
        inCircle.transform.position = spawnPos;
        outCircle.SetActive(true);
        inCircle.SetActive(true);
        bool isDone = false;

        outCircle.transform.localScale = originOutCircleScale;
        QTEJudge = CustomEnum.JudgeName.Miss;
        outCircle.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), (secondPerBeat*2) + (secondPerBeat / 20)).OnComplete(() => { isDone = true; }); //임시.. 비트에 맞게 수정요망
        while (!isDone)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
            {
                isDone = true;
                if (outCircle.transform.localScale.x >= perfectZone.x && outCircle.transform.localScale.x <= perfectZone.y) {
                    QTEJudge = JudgeName.Perfect;
                    Debug.Log("가드카운터 QTE 판정 Perfect");
                    break;
                }
                if (outCircle.transform.localScale.x >= greatZone.x && outCircle.transform.localScale.x <= greatZone.y){
                    QTEJudge = JudgeName.Great;
                    Debug.Log("가드카운터 QTE 판정 Great");
                    break;
                }
                if (outCircle.transform.localScale.x >= goodZone.x && outCircle.transform.localScale.x <= goodZone.y){
                    QTEJudge = JudgeName.Good;
                    Debug.Log("가드카운터 QTE 판정 Good");
                    break;
                }
                if (outCircle.transform.localScale.x >= missZone.x && outCircle.transform.localScale.x <= missZone.y){
                    QTEJudge = JudgeName.Miss;
                    Debug.Log("가드카운터 QTE 판정 Miss");
                    break;
                }
                
                break;
            }
            
            yield return null;

        }
        EndQTE();

    }
    private void EndQTE() {
        if (outCircle.activeSelf && inCircle.activeSelf)//qte가 활성화 상태라면 비활성화
        {
            outCircle.SetActive(false);
            inCircle.SetActive(false);
        }
    }
    

    
}
