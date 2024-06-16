using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionEffect : StateMachineBehaviour
{
    [SerializeField] private GameObject emotionParticlePrefab;
    [SerializeField] private string emotionSoundName;
    private GameObject emotionParticle;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(emotionParticle == null)
            emotionParticle = Instantiate(emotionParticlePrefab);
        if (emotionSoundName != null && emotionSoundName != string.Empty)
            AkSoundEngine.PostEvent(emotionSoundName, animator.gameObject);
        emotionParticle.transform.position = animator.transform.position;
        emotionParticle.SetActive(true);
    }
}
