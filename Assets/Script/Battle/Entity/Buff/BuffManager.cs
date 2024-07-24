using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager
{

    List<IBuff> buffList=new List<IBuff>();
    
    #region ������
    public BuffManager() { 
    
    
    }
    #endregion

    public void AddBuff(IBuff buff) {

        if (buffList.Contains(buff))
        {
            buffList[buffList.IndexOf(buff)].PlusStack(buff.GetStack());
        }
        else { 
        buffList.Add(buff);
        }
    
    }
    public void RemoveBuff(IBuff buff) { 
        
    
    }

    public void UpdateBuff() { 
        foreach (IBuff buff in buffList)
        {
            buff.UpdateStack();
            buff.CheckStack();

        }

    }

}
