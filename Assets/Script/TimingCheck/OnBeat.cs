using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class OnBeat : MonoBehaviour
{
    double currentTime=0d;
    const double MINUET_TO_SECOND=60d;
    int bPM = Metronome.instance.stage.BPM;
    public static event Action OnBeating;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {

        currentTime += Time.deltaTime;
        if (currentTime >= MINUET_TO_SECOND / bPM)
        {
            OnBeating();

            currentTime -= MINUET_TO_SECOND / bPM;
        }

    }
}
