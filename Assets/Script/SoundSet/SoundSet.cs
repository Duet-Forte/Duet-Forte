using UnityEngine;
using Util.CustomEnum;
namespace SoundSet
{

    public class PlayerSoundSet
    {
        //아래 해당 예시처럼 public void 사운드이름()으로 메서드를 만들고 안에 wwise 스크립트를 작성하시면 됩니다. 
        //주의할 점은 반드시 public void로 선언 해주시고 주석으로 어느 상황에서 쓰이는 사운드인지 상세히 적어주세용.


        //예시 코드 시작
        public void PerfectParrySound(GameObject parameterObject) { //퍼펙트 판정의 패링을 했을 시에 나오는 사운드

            AkSoundEngine.PostEvent("Player_Parry_SFX", parameterObject);

        }
        //예시코드 끝
       
        public void PlayerAttack(GameObject parameterObject,bool isSlash)//플레이어가 공격할 때 나는 사운드
        {
            if (isSlash)
            {
                AkSoundEngine.PostEvent("Player_Attack_A_SFX", parameterObject);
            }
            else {
                AkSoundEngine.PostEvent("Player_Attack_B_SFX", parameterObject);
            }

        }
        public void PlayerGuardCounter(GameObject parameterObject) { // 가드카운터 사운드
            AkSoundEngine.PostEvent("Player_Skill_Climax_SFX", parameterObject);
        }

      

    }

    public class EnemySoundSet
    { 
    }
    public class UISound {

        public void SignalSound(GameObject parameterObject,bool isStrong) {
            if (isStrong)
            {
                AkSoundEngine.PostEvent("Player_Attack_StrongSignal_SFX", parameterObject);
            }
            else {
                AkSoundEngine.PostEvent("Player_Attack_WeakSignal_SFX", parameterObject);
            }

        }
        public void AttackSignal(GameObject parameterObject, SignalInstrument signalInstrument) {
            AkSoundEngine.PostEvent($"Enemy_{signalInstrument.ToString()}_Attack_Signal_SFX", parameterObject);
        }
     
    }
    public class BackGroundMusic {

        public void GameOver(bool isWin) {
                Metronome.instance.GameOverMusic(isWin);
        }
    
    }

}