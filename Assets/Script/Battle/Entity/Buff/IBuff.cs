using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{

    public void PlusStack(int plus);
    //���� ������Ʈ
    public void UpdateStack();
    //���� �˻� - ������ �Ҹ���� ���� ����
    public bool CheckStack();
    public int GetStack();
}
