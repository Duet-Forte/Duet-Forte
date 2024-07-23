using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyData
{
    private string enemyName;//프리팹으로 하면 필요없음
    private int healthPoint;//프리팹으로하면 필용벗음
    private List<Pattern> pattern;//필요함
    private Vector2 spawnPoint;//필..요?
    private Sprite enemySprite;//필요없음.
    

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
