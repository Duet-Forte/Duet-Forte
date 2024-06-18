using Cysharp.Threading.Tasks;
using System;
using Util;
using UnityEngine;

public class TopViewEnemy : TopViewEntity
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] private int level;
    [SerializeField] private GameObject monsterEye;
    public event Action<string> OnFightPlayer;
    private PlayerTracker tracker;
    private BoxCollider2D boxCollider;
    public bool isFleeing;
    private void Update()
    {
        if (isAlive)
        {
            tracker.RequestPath(isFleeing);
        }
    }
    public override void InitSettings(string name, Vector2 intialSpawnPoint, int id = 0)
    {
        base.InitSettings(name, intialSpawnPoint, id);
        tracker = GetComponent<PlayerTracker>();
        tracker.InitSettings(this);
        boxCollider = GetComponent<BoxCollider2D>();
        OnFightPlayer -= BICSceneManager.Instance.SetBattleScene;
        OnFightPlayer += BICSceneManager.Instance.SetBattleScene;
    }

    public void Die()
    {
        RespawnTimer().Forget();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            BICSceneManager.Instance.Storage.currentBattleEnemy = this;
            OnFightPlayer?.Invoke(name);
        }
    }

    private async UniTask RespawnTimer()
    {
        isAlive = false;
        monsterEye.SetActive(false);
        boxCollider.enabled = false;
        await UniTask.Delay(7000);
        isAlive = true;
        monsterEye.SetActive(true);
        boxCollider.enabled = true;
    }

    public void Question() => Animator.Play(Const.questionHash);
    public void Surprised() => Animator.Play(Const.surpriseHash);
}
