using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{

    public void PlusStack(int plus);
    //스택 업데이트
    public void UpdateStack();
    //버프 검사 - 버프가 소멸될지 말지 결정
    public bool CheckStack();
    public int GetStack();
}
