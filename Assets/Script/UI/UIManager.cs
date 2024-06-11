using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UIManager
{
    private GameObject[] preFab;
    private StatusUIView statusUIView;
    private GameObject turnUIAsGameObject;
    private ControlTurnUI turnUI;
    private DamagePool damagePool;
    private Canvas canvas;
    private GameObject prepareTurnUI;
    private EnemySignalUI enemySignalUI;
    private BasicAttackQTE basicAttackQTE;

    public void StartStage(StageManager stageManager)
    {
        SetCanvas();
        if (preFab == null)
        {
            preFab = new GameObject[1];
            preFab[0] = Resources.Load<GameObject>(Const.UI_STATUS_PATH);
        }
        statusUIView = Object.Instantiate(preFab[0], canvas.transform).GetComponent<StatusUIView>();
        statusUIView.InitSettings(stageManager, canvas);
        GameObject.Instantiate(Resources.Load<GameObject>("UI/Status/UI_GuardCounterGauge"),canvas.transform);
        //statusUIView.GetOrAddComponent<ScreenSpaceCameraUI>().InitSettings(canvas);

        #region 현재 턴 표시하는 UI 세팅
        turnUIAsGameObject = Resources.Load<GameObject>(Const.UI_TURN);
        turnUI = Object.Instantiate(turnUIAsGameObject, canvas.transform).GetComponent<ControlTurnUI>();
        turnUI.InitSettings();
        stageManager.TurnUI = turnUI;
        #endregion

        prepareTurnUI = GameObject.Instantiate(Resources.Load<GameObject>("UI/PrepareTurnUI"),canvas.transform);
        stageManager.PrepareTurnUI = prepareTurnUI.GetComponent<PrepareTurnUI>();
        stageManager.PrepareTurnUI.InitSetting();

        
        basicAttackQTE= GameObject.Instantiate(Resources.Load<GameObject>("UI/BasicAttackQTE"), canvas.transform).GetComponent<BasicAttackQTE>();

        if (damagePool == null)
            damagePool = new DamagePool();
        damagePool.InitSettings(stageManager, canvas);
    }
    public void BasciAttackQTEControll(bool button) {
        if (button)
        {
            basicAttackQTE.StartQTE();
        }
        else { 
            basicAttackQTE.EndQTE();
        }
        
    
    }
    private void SetCanvas()
    {
        GameObject canvasObject = new GameObject("Stage UI Canvas");
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0;
        canvas.sortingOrder = 2;
        
        

    }

}
