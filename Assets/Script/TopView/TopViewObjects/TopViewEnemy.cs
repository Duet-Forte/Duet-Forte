using Cysharp.Threading.Tasks;
using System;
using Util;
using UnityEngine;

public class TopViewEnemy : TopViewEntity
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] private int level;
    public event Action<string> OnFightPlayer;
    private PlayerTracker tracker;
    public bool isFleeing;
    private void Update()
    {
        if (isAlive)
        {
            tracker.RequestPath(isFleeing);
        }
        else
        {
            
        }
    }
    public override void InitSettings(string name, Vector2 intialSpawnPoint, int id = 0)
    {
        base.InitSettings(name, intialSpawnPoint, id);
        tracker = GetComponent<PlayerTracker>();
        tracker.InitSettings(this);
        OnFightPlayer -= SceneManager.Instance.SetBattleScene;
        OnFightPlayer += SceneManager.Instance.SetBattleScene;
    }

    public void Die()
    {
        isAlive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            OnFightPlayer?.Invoke(name);
        }
    }

    private async UniTask RespawnTimer()
    {
        await UniTask.Delay(7000);

        isAlive = true;
    }

    public void Question() => Animator.Play(Const.questionHash);
    public void Surprised() => Animator.Play(Const.surpriseHash);
}
