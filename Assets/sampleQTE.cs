using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class sampleQTE : MonoBehaviour
{

    Canvas QTECanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        SetCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            DefenseQTE defQTE= GameObject.Instantiate(Resources.Load<DefenseQTE>("UI/UI_QTE"),QTECanvas.transform);
            defQTE.InitSetting(0.35f);
            defQTE.StartQTE();
            
        
        }
    }

    private void SetCanvas()
    {
        GameObject canvasObject = new GameObject("Stage UI Canvas");
        QTECanvas = canvasObject.AddComponent<Canvas>();
        QTECanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = QTECanvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0;
    }


}
