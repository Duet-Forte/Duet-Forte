using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffState
{
    //���� �߰�
    public void AddBuff(IBuff buff);
    //���� ����
    public void DeleteBuff(IBuff buff);

}
