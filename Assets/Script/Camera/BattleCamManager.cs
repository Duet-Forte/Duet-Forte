using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Cinemachine;

public class BattleCamManager : MonoBehaviour
{
    [SerializeField] CinemachineStateDrivenCamera stateDrivenCamera;
    private StageManager stageManager;
    [SerializeField] private CinemachineSmoothPath zoomInTrack;

    public Vector3 ZoomInTrack { set { zoomInTrack.transform.position = value;  } }
    public void InitSetting(StageManager stageManager) {
        
        stateDrivenCamera.m_AnimatedTarget = stageManager.PlayerInterface.PlayerTurn.gameObject.GetComponent<Animator>();
    
    }
}
