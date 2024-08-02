using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPattern
{
    [Tooltip("Entity 이름")]
    public string enemyName;

    [Tooltip("패턴 길이")]
    public int patternLength;

    [Tooltip("Entity 패턴")]
    public int[] patternArray;

    
}
public class PatternSet {

    public Vector2 line;
    public EnemyPattern[] enemyPatternes;

}
