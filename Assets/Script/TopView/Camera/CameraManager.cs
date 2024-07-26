using Cinemachine;
using UnityEngine;

public class CameraManager
{
    private Camera mainCamera;
    CinemachineVirtualCamera followCam, cutsceneCam;
    public CameraManager()
    {
        followCam = Object.Instantiate(Resources.Load<GameObject>("Camera/FollowCamera")).GetComponent<CinemachineVirtualCamera>();
        followCam.gameObject.SetActive(false);
        mainCamera = Camera.main;
    }

    public void SetFollowCamera()
    {
        cutsceneCam.gameObject.SetActive(false);
        followCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 15f;
        followCam.Follow = GameManager.FieldManager.Player.transform;
        followCam.gameObject.SetActive(true);
    }

    public void ChangeCameraCollider(string regionName)
    {
        followCam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = 
            GameManager.FieldManager.Field.GetCameraCollider(regionName);
    }

    public void SetCutsceneCamera(CinemachineVirtualCamera cutsceneCam) => this.cutsceneCam = cutsceneCam;
    public void DisableFollowCamera() => followCam.gameObject.SetActive(false);
    public void EnableFieldCamera() => mainCamera.gameObject.SetActive(true);
}
