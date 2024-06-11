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
        switchingActive(false);
    }
    private void InitSettings() {
        inCircle=gameObject.transform.Find("InCircle").gameObject;
        outCircle = gameObject.transform.Find("OutCircle").gameObject;
        backGround = gameObject.transform.Find("BackGround").gameObject;
        

    }
    public void StartQTE() {
        StartCoroutine(RunQTE());
    }
    private IEnumerator RunQTE() {
        switchingActive(true);
        float dynamicScale;
        
        isQte = true;
        //서클 활성화 
        Metronome.instance.OnBeating += SubscribeQTE;
        yield return null;

        
    }


    private IEnumerator OnBeatQTE() {
        Metronome.instance.OnBeating += ShakeCamera;
        while (isQte)
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.F))
            {
                Metronome.instance.OnBeating -= ShakeCamera;
                EndQTE();
                break;
            }
            if (Metronome.instance.CurrentTime <= secondPerBeat * 0.5f)
            {

                outCircle.transform.DOScale(new Vector2(2, 2), secondPerBeat * 0.5f);
            }
            if (Metronome.instance.CurrentTime > secondPerBeat * 0.5f)
            {

                outCircle.transform.DOScale(new Vector2(1, 1), secondPerBeat * 0.5f);
            }

            yield return new WaitForFixedUpdate();
        }

    }
    private void SubscribeQTE() {
        StartCoroutine(OnBeatQTE());
        Metronome.instance.OnBeating -= SubscribeQTE;
    }

    private void ShakeCamera() {
        battleDirector.CameraShake(0.1f, 0.2f, 10, 30);
    }


    
    public void EndQTE() {
        //인서클 아웃서클 원본크기로 두고 비활성화
        outCircle.transform.localScale = new Vector2(1, 1);
        switchingActive(false);
        Metronome.instance.OnBeating -= ShakeCamera;
    }
    private void switchingActive(bool onOff) {
        inCircle.SetActive(onOff);
        outCircle.SetActive(onOff);
        backGround.SetActive(onOff);

    }
    public void InvokeQTE() { 
    
    }

}
