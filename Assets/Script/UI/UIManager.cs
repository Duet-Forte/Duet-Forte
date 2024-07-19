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
    private StageClear stageClear;
    private StageManager stageManager;
    private SkillPopUpUI skillPopUpUI;

    public void StartStage(StageManager stageManager)
    {
        this.stageManager = stageManager;
        SetCanvas();
        if (preFab == null)
        {
            preFab = new GameObject[1];
            preFab[0] = Resources.Load<GameObject>(Const.UI_STATUS_PATH);
        }
        statusUIView = Object.Instantiate(preFab[0], canvas.transform).GetComponent<StatusUIView>();
        statusUIView.InitSettings(stageManager, canvas);
        //GameObject.Instantiate(Resources.Load<GameObject>("UI/Status/UI_GuardCounterGauge"),canvas.transform);
        //statusUIView.GetOrAddComponent<ScreenSpaceCameraUI>().InitSettings(canvas);

        

        prepareTurnUI = GameObject.Instantiate(Resources.Load<GameObject>("UI/PrepareTurnUI"),canvas.transform);
        stageManager.PrepareTurnUI = prepareTurnUI.GetComponent<PrepareTurnUI>();
        stageManager.PrepareTurnUI.InitSetting();

        skillPopUpUI = GameObject.Instantiate(Resources.Load<GameObject>("UI/SkillPopUpUI"), canvas.transform).GetComponent<SkillPopUpUI>();
        

        basicAttackQTE= GameObject.Instantiate(Resources.Load<GameObject>("UI/BasicAttackQTE"), canvas.transform).GetComponent<BasicAttackQTE>();
        stageClear = GameObject.Instantiate(Resources.Load<GameObject>("UI/StageClear"), canvas.transform).GetComponent<StageClear>();
        stageClear.InitSettings(stageManager);

        #region ���� �� ǥ���ϴ� UI ����
        turnUIAsGameObject = Resources.Load<GameObject>(Const.UI_TURN);
        turnUI = Object.Instantiate(turnUIAsGameObject, canvas.transform).GetComponent<ControlTurnUI>();
        turnUI.InitSettings();
        stageManager.TurnUI = turnUI;
        #endregion

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
    public void AppearSkillPopUp(Sprite skillImage,string skillName) {
        skillPopUpUI.Appear(skillImage, skillName);
    }
    public void DisAppearSkillPopUp() {
        skillPopUpUI.Disappear();
    }
    public void InvokeGameClear() {
        Debug.Log("UIManager���� InvokeGameClearȣ���");
        
        stageClear.Invoke(stageManager.TurnCount, true, stageManager.Enemy.Exp);
    }
    public void InvokeGameOver() {
        stageClear.Invoke(stageManager.TurnCount, false, stageManager.Enemy.Exp);
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
