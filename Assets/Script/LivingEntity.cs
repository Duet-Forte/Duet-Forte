using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    // Start is called before the first frame update
    public int startingHp=70;//ü��
    public int health { get; protected set; }
    public int defense;//����
    public int attack;//���ݷ�

    public virtual void OnDamage() { 
    
    
    }

}
