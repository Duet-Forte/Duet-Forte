using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{

    PlayerAnimator playerAnimator;
    PlayerStatus playerStatus;
    PlayerTurn playerTurn;
    PlayerAttack playerAttack;
    PlayerGuardCounter playerGuardCounter;
    Cinemachine.CinemachineImpulseSource cinemachineImpulseSource;

    

    #region 위치 관련 변수
    Vector3 parryPos = new Vector3(2.61f, 1.54f, 0f);
    #endregion

    #region 프로퍼티
    public PlayerAnimator PlayerAnimator { get {
            if (playerAnimator != null)
            { 
                return playerAnimator;
            }
                    return GetComponent<PlayerAnimator>(); 
        } 
    }

    public PlayerStatus PlayerStatus
    {
        get
        {
            if (playerStatus != null)
            {
                return playerStatus;
            }
            return GetComponent<PlayerStatus>();
        }
    }
    public PlayerTurn PlayerTurn
    {
        get
        {
            if (playerTurn != null)
            {
                return playerTurn;
            }
            return GetComponent<PlayerTurn>();
        }
    }
    public PlayerAttack PlayerAttack
    {
        get
        {
            if (playerAttack != null)
            {
                return playerAttack;
            }
            return GetComponent<PlayerAttack>();
        }
    }
    public PlayerGuardCounter PlayerGuardCounter
    {
        get 
        {
            if (playerGuardCounter != null)
            {
                return playerGuardCounter;
            }
            return GetComponent<PlayerGuardCounter>();
        }
    
    
    }

    #region 파티클 생성 위치
    public Vector3 ParryPos { get => parryPos; }

    #endregion

    public void GetDamage(int damage) { 
    
    }

    public Cinemachine.CinemachineImpulseSource CinemachineImpulseSource
    {
        get
        {
            if (cinemachineImpulseSource != null)
            {
                return cinemachineImpulseSource;
            }
            return GetComponent<Cinemachine.CinemachineImpulseSource>();
        }
    }
    #endregion

    
    void Awake()
    {
        playerTurn = GetComponent<PlayerTurn>();
        playerAttack = GetComponent<PlayerAttack>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerStatus = GetComponent<PlayerStatus>();
        cinemachineImpulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();

    }

    
}
