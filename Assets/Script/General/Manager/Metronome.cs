using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Util;

public class Metronome : MonoBehaviour
{
    public static Metronome instance = null;
    public Stage stage; //bpm�̳� bgm���� �� stage�� ���� ����

    private double currentTime = 0d;//���ڼ��� ����
    private double currentTimeOfOffBeat = 0d;//���ڼ��� ����
    
    int bPM;
    double secondsPerBeat = 0;
    
    public event Action OnBeating;
    public event Action OffBeating;

    public double CurrentTime { get => currentTime; }
    public double SecondsPerBeat { get => secondsPerBeat; }
    
    private void Awake()//�̱���
    {
        if (instance == null)
        {
            instance = this;

        }
        else {
            if (instance != this)
                Destroy(this.gameObject);
        
        }
        bPM = stage.BPM;
        secondsPerBeat = Const.MINUTE_TO_SECOND / bPM;
    }


   /* void Start()
    {
        bPM = instance.stage.BPM;
    }*/

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        currentTimeOfOffBeat += Time.deltaTime;
        if (currentTimeOfOffBeat >= secondsPerBeat*0.5) {
            
            currentTimeOfOffBeat -= secondsPerBeat;
            OffBeating?.Invoke();
        }
        if (currentTime >= secondsPerBeat)
        {
            
            currentTime -= secondsPerBeat;
            OnBeating?.Invoke();
        }
    }
}
