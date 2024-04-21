using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Object/Stage", order = 1)]
public class Stage : ScriptableObject
{
    //���������� ���� ���� bpm, enemy�� ���� ����, bgm, ��� �Ϸ���Ʈ, 
    [SerializeField] private int id;
    [SerializeField] private int bpm;
    [SerializeField] private EnemyData enemy;
    [SerializeField] private string enemyName; // �ش� �̸��� �Ȱ��� �̸����� Object/Enemy�� ���������� ����� ��


    public int ID { get => id; set => id = value; }
    public int BPM { get => bpm; set => bpm = value; }
    public EnemyData Enemy { get => enemy;}
    public string EnemyName { get => enemyName; set => enemyName = value; }
    #region ���
    private const int MINUET_TO_SECOND = 60;

    #endregion
}
