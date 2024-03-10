using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Object/Stage", order = 1)]
public class Stage : ScriptableObject
{
    //스테이지에 대한 정보 bpm, enemy에 대한 정보, bgm, 배경 일러스트, 
    [SerializeField] private int id;
    [SerializeField] private int bpm;
    [SerializeField] private EnemyData enemy;
    [SerializeField] private string enemyName; // 해당 이름과 똑같은 이름으로 Object/Enemy에 프리팹으로 만들면 됨


    public int ID { get => id; set => id = value; }
    public int BPM { get => bpm; set => bpm = value; }
    public EnemyData Enemy { get => enemy;}
    public string EnemyName { get => enemyName; set => enemyName = value; }
    #region 상수
    private const int MINUET_TO_SECOND = 60;

    #endregion
}
