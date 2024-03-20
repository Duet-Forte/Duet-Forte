using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Util;

/// <summary>
/// �� ��ũ��Ʈ�� ������Ʈ������ ������ �ٷ�� �ֽ��ϴ�.
/// ������Ʈ�� �ǰݽ� �ڷ� �и��ٰų� ĳ���Ͱ� ��鸰�ٴ��� ���ݽ� ������ �̵��ϴ� ���� 
/// 
/// </summary>
namespace Director
{
    public class BattleDirector
    {

        public void KnockBack(GameObject target) {

            target.transform.DOMove(new Vector3(target.transform.position.x, target.transform.position.y), 0.1f).SetEase(Ease.OutQuart);



        }
        public void Shake(GameObject target)
        {

            target.transform.DOShakePosition(0.3f, 0.5f, 100, 30);

        }
        public void SlowMotion(float duration) {
            int normalTimeSpeed = 1;
            float slowTimeSpeed = 0.3f;

            Time.timeScale = slowTimeSpeed;

            //Time.timeScale = DOTween.To(() => slowTimeSpeed, x => slowTimeSpeed = x, normalTimeSpeed, duration);


        }
        public void Rush(GameObject target, Vector2 endValue, float duringTime,Ease ease) {

            target.transform.DOLocalMove(endValue, duringTime).SetEase(ease);
        
        }

    }

    public class SpacialAttack {

        string playerGuardCounterSlashParticle = "VFX/VFX_Prefab/Combat/Player/GuardCounter/Player_CounterAttack_GuardCounter_Slash_VFX";
        public void GenerateGuardCounterSlash(GameObject target) {
            GameObject parryParticle = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(playerGuardCounterSlashParticle));
            parryParticle.transform.localPosition = target.transform.position;
            

        }

}

    public class HitParticle {

        string hitParticlePath = "VFX/VFX_Prefab/Combat/Player/Guard";
        
        string playerParryingPerfectPath = "VFX/VFX_Prefab/Combat/Player/Guard/Player_Parrying_PerfectGuard_VFX";
        string playerParryingNonPerfectPath = "VFX/VFX_Prefab/Combat/Player/Guard/Player_Parry_NonPerfect_VFX";


        public void GenerateParryHit(GameObject target,bool isPerfect) {

            if (isPerfect)
            {
                GameObject parryParticle = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(playerParryingPerfectPath));
                parryParticle.transform.SetParent(target.transform);
                parryParticle.transform.localPosition = new Vector3(2.61f, 1.54f, 0f);
            }
            if (!isPerfect) {
                GameObject parryParticle = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(playerParryingNonPerfectPath));
                parryParticle.transform.SetParent(target.transform);
                parryParticle.transform.localPosition = new Vector3(2.61f, 1.54f, 0f);
            }
        }
        public void GenerateNonePerfectParryHit(GameObject player) {
            GameObject parryParticle = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(playerParryingNonPerfectPath));
            parryParticle.transform.SetParent(player.transform);
            parryParticle.transform.localPosition = new Vector3(2.61f, 1.54f, 0f);

        }
        public void Generate_Player_Hit_Slash(Transform target) { 
            ParticleSystem playerHitSlash = GameObject.Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("VFX/VFX_Prefab/Combat/Player/Hit/Player_Hit_Slash01_VFX"));
            playerHitSlash.transform.SetParent(target);
            playerHitSlash.transform.localPosition = Vector3.zero;

        }
        public void Generate_Player_Hit_Pierce(Transform target)
        {
            ParticleSystem playerHitSlash = GameObject.Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("VFX/VFX_Prefab/Combat/Player/Hit/Player_Hit_Poke01_VFX"));
            playerHitSlash.transform.SetParent(target);
            playerHitSlash.transform.localPosition = Vector3.zero;

        }



    }
}
