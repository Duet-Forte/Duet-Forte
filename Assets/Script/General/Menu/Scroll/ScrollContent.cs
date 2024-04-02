
using UnityEngine;
using UnityEngine.Pool;

public abstract class ScrollContent : MonoBehaviour
{
    public ObjectPool<ScrollContent> pool;
    public void InitSetting(ObjectPool<ScrollContent> pool) => this.pool = pool;
    public void ReleaseSelf() 
    { 
        gameObject.SetActive(false); 
        pool.Release(this);
    }
    public abstract void RefreshContent(int rowIndex);
}
