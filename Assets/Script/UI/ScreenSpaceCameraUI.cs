using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceCameraUI : MonoBehaviour
{
    public void InitSettings(Canvas canvas = null)
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        
        canvas.worldCamera = Camera.main;
    }
}
