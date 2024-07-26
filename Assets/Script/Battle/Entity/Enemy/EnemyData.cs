using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyData
{
    private string enemyName;//���������� �ϸ� �ʿ����
    private int healthPoint;//�����������ϸ� �ʿ����
    private List<Pattern> pattern;//�ʿ���
    private Vector2 spawnPoint;//��..��?
    private Sprite enemySprite;//�ʿ����.
    

    public string Name { get { return enemyName; } set { enemyName = value; } }
    public int HP { get { return healthPoint; } set { healthPoint = value; } }
    public Vector2 SpawnPoint { get { return spawnPoint; } set { spawnPoint = value; } }
    public List<Pattern> Pattern
    {
        get
        {
            if (pattern == null)
            {
                pattern = new List<Pattern>();
            }
            return pattern;
        }
        set { pattern = value; }
    }
    public Sprite Sprite { get { return enemySprite; } set { enemySprite = value; } }

}
