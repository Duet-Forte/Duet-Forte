using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnUI : MonoBehaviour, IStageUI
{
    private StageManager stageManager;
    private Canvas canvas;
    public void InitSettings(StageManager stageManager, Canvas canvas) {
        this.stageManager = stageManager;
        this.canvas = canvas;
        
    }
    public void PrepareTurnStart() { 
        
        
    }
    
}
