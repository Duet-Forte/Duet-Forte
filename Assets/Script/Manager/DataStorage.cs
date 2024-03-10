using Mono.CompilerServices.SymbolWriter;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬이 넘어가거나 게임이 종료되어도 저장되는 데이터
/// 추후에 json으로 파일 출력 해서 저장 예정.
/// </summary>
public class DataStorage
{
    private Stage currentStage;
    public Stage Stage { get => currentStage; }

    private Vector2 playerPosition;
    public Vector2 PlayerPosition { get => playerPosition; set => playerPosition = value; }

    public Vector2[] entityPosition;
    public Vector2[] EntityPosition { get => entityPosition; set => entityPosition = value; }
    private Field currentField;
    public Field Field { get => currentField; set => currentField = value; }
    private Dictionary<string, Stage> stages;

    public void InitSettings()
    {
        Stage[] stageFiles = Resources.LoadAll<Stage>(Util.Const.STAGE_PATH);
        foreach (Stage stage in stageFiles)
        {
            stages.Add(stage.name, stage);
        }
    }
    public void SetStage(string name)
    {
        if (stages[name] != null)
            currentStage = stages[name];
        else
            Debug.Log("파일을 찾을 수 없습니다.");
    }
}
