using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Director;
using DG.Tweening;

public class BasicAttackQTE : MonoBehaviour
{

    private GameObject inCircle;
    private GameObject outCircle;
    private GameObject backGround;
    private bool isQte = true;
    private BattleDirector battleDirector=new BattleDirector();
    private float secondPerBeat;
    // Start is called before the first frame update
    private void OnEnable()
    {
        InitSettings();
        secondPerBeat = (float)Metronome.instance.SecondsPerBeat;
        StartCoroutine(StatQTE());
    }
    public void InitSettings() {
        inCircle=gameObject.transform.Find("InCircle").gameObject;
        outCircle = gameObject.transform.Find("OutCircle").gameObject;
        backGround = gameObject.transform.Find("BackGround").gameObject;

        
    }
    public IEnumerator StatQTE() {
        switchingActive(true);
        float dynamicScale;
        Metronome.instance.OnBeating += ShakeCamera;
        isQte = true;
        //서클 활성화 
        while (isQte) {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.F)) { 
                Metronome.instance.OnBeating-= ShakeCamera;
                break;
            }
            if (Metronome.instance.CurrentTime <= secondPerBeat*0.5f) {
                //dynamicScale=Mathf.Lerp(1, 2, (float)Metronome.instance.CurrentTime / secondPerBeat * 0.5f);
                //outCircle.transform.localScale = new Vector2(dynamicScale, dynamicScale);
                outCircle.transform.DOScale(new Vector2(2, 2), secondPerBeat * 0.5f);
            }
            if (Metronome.instance.CurrentTime > secondPerBeat * 0.5f) {
                //dynamicScale = Mathf.Lerp(2, 1,((float)Metronome.instance.CurrentTime-secondPerBeat*0.5f) / secondPerBeat*0.5f);
                //outCircle.transform.localScale=new Vector2(dynamicScale, dynamicScale); 
                outCircle.transform.DOScale(new Vector2(1, 1), secondPerBeat * 0.5f);
            }
            
            yield return null;
        }

        
    }
    private void ShakeCamera() {
        battleDirector.CameraShake(0.3f, 0.3f, 10, 30);
    }


    
    public void EndQTE() {
        //인서클 아웃서클 원본크기로 두고 비활성화
        outCircle.transform.localScale = new Vector2(1, 1);
        switchingActive(false);
    }
    private void switchingActive(bool onOff) {
        inCircle.SetActive(onOff);
        outCircle.SetActive(onOff);
        backGround.SetActive(onOff);

    }
    public void InvokeQTE() { 
    
    }

}
