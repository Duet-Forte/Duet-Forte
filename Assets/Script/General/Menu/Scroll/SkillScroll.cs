using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScroll : ScrollPool
{

    public GameObject[] test = new GameObject[10];
    private void Start()
    {
        InitSettings(test);
    }
}
