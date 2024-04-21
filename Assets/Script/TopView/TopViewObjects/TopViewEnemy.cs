using System;
using Unity.VisualScripting;
using UnityEngine;

public class TopViewEnemy : TopViewEntity
{
    private bool isAlive = true;
    public event Action<string> OnFightPlayer; 

    public override void InitSettings(string name, Vector2 intialSpawnPoint, int id = 0)
    {
        base.InitSettings(name, intialSpawnPoint, id);
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
}
