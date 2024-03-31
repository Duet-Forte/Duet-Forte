using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SkillAnimation", menuName = "Scriptable Object/SkillAnimation", order = 3)]
public class SkillAnimation : ScriptableObject
{
    [SerializeField] private AnimationClip clip;
    [SerializeField] private ParticleSystem[] skillParticles;
    int particleCount = 0;

    public AnimationClip Clip { get => clip; }
    public ParticleSystem[] SkillParticles { get => skillParticles; }
    void PlayParticles() {
        ParticleSystem parryParticle = GameObject.Instantiate<ParticleSystem>(skillParticles[particleCount]);
        //parryParticle.transform.SetParent(.transform);
        skillParticles[particleCount].Play();
        particleCount++;
        if (particleCount == skillParticles.Length) particleCount = 0;
    }
}
