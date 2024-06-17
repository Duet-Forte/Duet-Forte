using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataBase
{
    private Dictionary<string, Stage> stages;
    public StageDataBase()
    {
        stages = new Dictionary<string, Stage>();
        Stage[] stageData = Resources.LoadAll<Stage>("TopView/Stages");
        foreach (Stage stage in stageData)
        {
            stages.Add(stage.EnemyName, stage);
        }
    }

    public Stage GetStage(string stageName)
    {
        return stages[stageName];
    }
}
