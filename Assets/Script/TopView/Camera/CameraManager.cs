using Cinemachine;
using UnityEngine;

public class CameraManager
{
    private Camera fieldCamera;
    private CinemachineVirtualCamera virtualCamera;
    public CinemachineVirtualCamera CutsceneCamera { get => virtualCamera; }
    public void InitSetting()
    {
        virtualCamera = Camera.main.GetComponent<CinemachineVirtualCamera>();
    }
    
}
