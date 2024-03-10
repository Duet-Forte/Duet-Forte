using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    // Start is called before the first frame update
    public int startingHp=70;//체력
    public int health { get; protected set; }
    public int defense;//방어력
    public int attack;//공격력

    public virtual void OnDamage() { 
    
    
    }

}
