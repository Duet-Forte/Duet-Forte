using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    //�� ���ڵ��� ���� 4���� 4���ڸ� �������� ��.

    int bPM = 135; // stage ��ü���� bpm�� ������ ����
    private const double MINUTE_TO_SECOND = 60d;
    private double whole; //����ǥ
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
