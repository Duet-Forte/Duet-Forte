using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    //본 박자들은 전부 4분의 4박자를 기준으로 함.

    int bPM = 135; // stage 객체에서 bpm을 참조할 예정
    private const double MINUTE_TO_SECOND = 60d;
    private double whole; //온음표
    private double half;
    private double quarter;
    private double eight;
    private double sixteen;

    public double WholeTime { get => whole; }
    public double HalfTime { get => half; }
    public double QuarterTime { get => quarter; }
    public double EightTime { get => eight; }
    public double SixteenTime { get => sixteen; }

    // Start is called before the first frame update
    void Start()
    {

        // bitPerMinute=Stage.bitPerMinute;
        whole = MINUTE_TO_SECOND * 4 / bPM;
        half= MINUTE_TO_SECOND * 2 / bPM;
        quarter= MINUTE_TO_SECOND/ bPM;
        eight= MINUTE_TO_SECOND * 0.5 / bPM;
        sixteen = MINUTE_TO_SECOND * 0.25 / bPM;
        
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
