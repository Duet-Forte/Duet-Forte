using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Util;
using UnityEngine.Events;
using AK.Wwise;

public class Metronome :MonoBehaviour
{
    public static Metronome instance = null;
    private Stage stage; //bpm�̳� bgm���� �� stage�� ���� ����

    private double currentTime = 0d;//���ڼ��� ����
    private double currentTimeOfOffBeat = 0d;//���ڼ��� ����
    
    int bPM;
    double secondsPerBeat = 0;
    
    public event Action OnBeating;
    public event Action OffBeating;

    #region wwise callbackFunc
    public string MusicEvent;
    public string MuisicStopEvent;

    public UnityEvent KickEvent;
    private Unit musicEventPlayingID;
    #endregion

    public double CurrentTime { get => currentTime; }
    public double SecondsPerBeat { get => secondsPerBeat; }
    public Stage getStage { get => stage; }
    public void InitSettins(Stage stage)//�̱���
    {
        if (instance == null)
        {
            instance = this;

        }
        else {
            if (instance != this)
            {
              //Destroy(this.gameObject);
            }
        
        }
        bPM = stage.BPM;
        secondsPerBeat = Const.MINUTE_TO_SECOND / bPM;

        MusicStart();

        this.stage = stage;
        //AkSoundEngine.PostEvent();
    }
    public void WwiseCallbackEvent() { Debug.Log("Kick"); }
    private void InvokeOnBeating()
    {
        //musicEventPlayingID = AkSoundEngine.PostEvent(MusicEvent, gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, SetKickEvent, this);
    }
    public void SetKickEvent(object in_cookie, AkCallbackType in_type, object in_callbackInfo) {
        AkMusicSyncCallbackInfo musicSyncInfo = in_callbackInfo as AkMusicSyncCallbackInfo;

        if (musicSyncInfo == null) {
            return;
        }
        KickEvent.Invoke();
        Debug.Log("Kick Happended");
    }
    private void MusicStart() { 

        AkSoundEngine.PostEvent("Combat_Stage_01_BGM", gameObject);
        AkSoundEngine.SetSwitch("Stage_01", "Stage_01_Start", gameObject);
        
    }
    public void GameOverMusic(bool isWin) {
        if (isWin)
        {
            AkSoundEngine.PostEvent("Combat_BGM_Stop", gameObject);
            AkSoundEngine.SetSwitch("Stage_01", "Stage_01_End", gameObject);
            AkSoundEngine.PostEvent("Combat_Stage_01_BGM", gameObject);
        }
        else { 
            //�й�� BGM
        }
        
    }
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
