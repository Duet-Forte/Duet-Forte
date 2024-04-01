
using UnityEngine;
using UnityEngine.Pool;

public class ScrollContent : MonoBehaviour
{
    public ObjectPool<ScrollContent> pool;
    public void InitSetting(ObjectPool<ScrollContent> pool) => this.pool = pool;
    public void ReleaseSelf() 
    { 
        gameObject.SetActive(false); 
        pool.Release(this);
    }
}
