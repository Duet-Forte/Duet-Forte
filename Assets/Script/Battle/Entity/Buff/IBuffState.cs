using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffState
{
    //버프 추가
    public void AddBuff(IBuff buff);
    //버프 제거
    public void DeleteBuff(IBuff buff);

}
