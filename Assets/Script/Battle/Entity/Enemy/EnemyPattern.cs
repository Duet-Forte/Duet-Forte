using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPattern
{
    [Tooltip("Entity �̸�")]
    public string enemyName;

    [Tooltip("���� ����")]
    public int patternLength;

    [Tooltip("Entity ����")]
    public int[] patternArray;

    
}
public class PatternSet {

    public Vector2 line;
    public EnemyPattern[] enemyPatternes;

}
